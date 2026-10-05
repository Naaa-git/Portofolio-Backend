using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/experiences")]
public class ExperiencesController(IExperienceService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ExperienceDto>>> GetAll([FromQuery] string lang = "id") =>
        Ok(await service.GetAllAsync(lang));

    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<List<ExperienceAdminDto>>> GetAllAdmin() => Ok(await service.GetAllAdminAsync());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ExperienceAdminDto>> Create(ExperienceUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, ExperienceUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
