using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController(IProfileService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProfileDto>> Get([FromQuery] string lang = "id")
    {
        var profile = await service.GetAsync(lang);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<ProfileAdminDto>> GetAdmin()
    {
        var profile = await service.GetAdminAsync();
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<ProfileAdminDto>> Upsert(ProfileUpsertDto dto) =>
        Ok(await service.UpsertAsync(dto));
}
