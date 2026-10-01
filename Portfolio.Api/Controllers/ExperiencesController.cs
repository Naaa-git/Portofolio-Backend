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
    public async Task<ActionResult<List<ExperienceDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ExperienceDto>> Create(ExperienceUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, ExperienceUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
