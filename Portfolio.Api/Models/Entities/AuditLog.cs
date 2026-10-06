namespace Portfolio.Api.Models.Entities;

public enum AuditAction { Create, Update, Delete }

/// <summary>
/// One row per entity write made through the admin panel. Populated automatically
/// by AuditSaveChangesInterceptor — nothing in the Services/Controllers layer
/// writes to this directly, so new entities get audited for free.
/// </summary>
public class AuditLog
{
    public int Id { get; set; }
    public DateTime TimestampUtc { get; set; }

    /// <summary>Username of the logged-in admin, taken from the JWT at save time. Null for unauthenticated writes (e.g. the startup seeder).</summary>
    public string? AdminUsername { get; set; }

    public AuditAction Action { get; set; }
    public string EntityName { get; set; } = default!;
    public string EntityId { get; set; } = default!;

    /// <summary>JSON: { "FieldName": { "old": ..., "new": ... } }. Null for Create/Delete where there's nothing to diff.</summary>
    public string? Changes { get; set; }
}
