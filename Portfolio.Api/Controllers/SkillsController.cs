using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillsController(ISkillService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SkillDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SkillDto>> Create(SkillUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, SkillUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
