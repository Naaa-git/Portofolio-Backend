using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IAuditLogService
{
    Task<List<AuditLogDto>> GetRecentAsync(int limit = 200);
}
