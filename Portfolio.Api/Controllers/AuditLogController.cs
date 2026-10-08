using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/audit-log")]
[Authorize]
public class AuditLogController(IAuditLogService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AuditLogDto>>> GetRecent([FromQuery] int limit = 200) =>
        Ok(await service.GetRecentAsync(limit));
}
