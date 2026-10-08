using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Data;

/// <summary>
/// Hooks into EF Core's SaveChanges pipeline and writes one AuditLog row per
/// entity that was created/updated/deleted in the same transaction — covers
/// every entity automatically, so nothing needs to remember to call an
/// "AuditService.Log(...)" by hand in each Service method.
///
/// Registered as Scoped (one instance per request/DbContext), so it's safe to
/// keep state on instance fields between the Saving and Saved callbacks below.
///
/// Two phases, because of a timing conflict:
///   - A newly-created row's real database ID is only known AFTER the insert
///     commits (before that, EF Core only has a temporary placeholder key).
///   - An updated row's "old" values are only readable BEFORE the update
///     commits (after that, Postgres already has the new values).
/// So: diff Modified/Deleted entities before save, fill in Added entities'
/// real IDs after save, then persist the batch in a second, separate save.
/// </summary>
public class AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    // AuditLog itself would otherwise audit its own insert (infinite loop).
    // AdminUser is skipped so password hashes / TOTP secrets never end up in a log.
    private static readonly HashSet<Type> ExcludedTypes = [typeof(AuditLog), typeof(AdminUser)];

    private readonly List<AuditLog> _pendingLogs = [];
    private readonly List<(EntityEntry Entry, AuditLog Log)> _pendingCreates = [];

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PrepareAuditLogsAsync(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // The repositories in this app are all async, so this sync path is never
        // actually exercised — but SaveChangesInterceptor requires overriding it.
        PrepareAuditLogsAsync(eventData.Context, default).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await FinalizeAndPersistAuditLogsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        FinalizeAndPersistAuditLogsAsync(eventData.Context, default).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    private async Task PrepareAuditLogsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        _pendingLogs.Clear();
        _pendingCreates.Clear();
        if (context is null) return;

        var username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        var now = DateTime.UtcNow;

        // Snapshot first: adding AuditLog entities to the context while iterating
        // ChangeTracker.Entries() would mutate the collection we're looping over.
        var entries = context.ChangeTracker.Entries()
            .Where(e => !ExcludedTypes.Contains(e.Entity.GetType())
                && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            .ToList();

        foreach (var entry in entries)
        {
            var log = new AuditLog
            {
                TimestampUtc = now,
                AdminUsername = username,
                EntityName = entry.Entity.GetType().Name,
                // Added entities don't have their real (database-generated) ID yet —
                // filled in for real once SavedChangesAsync runs, after the insert commits.
                EntityId = entry.State == EntityState.Added ? "" : GetEntityId(entry),
                Action = entry.State switch
                {
                    EntityState.Added => AuditAction.Create,
                    EntityState.Deleted => AuditAction.Delete,
                    _ => AuditAction.Update,
                },
                Changes = entry.State == EntityState.Modified
                    ? await BuildChangesJsonAsync(entry, cancellationToken)
                    : null,
            };

            _pendingLogs.Add(log);
            if (entry.State == EntityState.Added)
            {
                _pendingCreates.Add((entry, log));
            }
        }
    }

    private async Task FinalizeAndPersistAuditLogsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null || _pendingLogs.Count == 0) return;

        foreach (var (entry, log) in _pendingCreates)
        {
            log.EntityId = GetEntityId(entry); // now populated with the real DB-generated key
        }

        context.Set<AuditLog>().AddRange(_pendingLogs);
        // A second, separate save — SavingChangesAsync runs again for it, but
        // the only pending changes now are these AuditLog inserts, which are
        // filtered out by ExcludedTypes, so this doesn't recurse.
        await context.SaveChangesAsync(cancellationToken);

        _pendingLogs.Clear();
        _pendingCreates.Clear();
    }

    private static string GetEntityId(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key is null) return "";
        var values = key.Properties.Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? "null");
        return string.Join(",", values);
    }

    /// <summary>
    /// Several repositories in this app attach a freshly-constructed object and
    /// call Update() directly, without loading the row first — EF Core then has
    /// no real "before" snapshot to diff against (OriginalValue just mirrors
    /// CurrentValue). GetDatabaseValuesAsync() queries Postgres for what's
    /// actually stored right now (this runs before the transaction commits, so
    /// it's still the pre-update row) and we diff against that instead.
    /// </summary>
    private static async Task<string?> BuildChangesJsonAsync(EntityEntry entry, CancellationToken cancellationToken)
    {
        var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
        if (databaseValues is null) return null; // row no longer exists (shouldn't happen for an Update)

        var changes = new Dictionary<string, object?>();

        foreach (var property in entry.Properties)
        {
            var oldValue = databaseValues[property.Metadata];
            var newValue = property.CurrentValue;

            // List<string>/Dictionary<string,string> properties (TechStack, the
            // translatable {id, en} fields, ...) don't define value-based Equals,
            // so two structurally-identical instances compare unequal by
            // reference. Comparing their JSON form instead makes this a real
            // "did the content change" check for every property type uniformly.
            var oldJson = JsonSerializer.Serialize(oldValue);
            var newJson = JsonSerializer.Serialize(newValue);
            if (oldJson == newJson) continue;

            changes[property.Metadata.Name] = new { old = oldValue, @new = newValue };
        }

        return changes.Count == 0 ? null : JsonSerializer.Serialize(changes);
    }
}
