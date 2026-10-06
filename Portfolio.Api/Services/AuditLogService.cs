using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public class AuditLogService(AppDbContext db) : IAuditLogService
{
    public async Task<List<AuditLogDto>> GetRecentAsync(int limit = 200) =>
        await db.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.TimestampUtc)
            .Take(limit)
            .Select(a => new AuditLogDto(a.Id, a.TimestampUtc, a.AdminUsername, a.Action.ToString(), a.EntityName, a.EntityId, a.Changes))
            .ToListAsync();
}
