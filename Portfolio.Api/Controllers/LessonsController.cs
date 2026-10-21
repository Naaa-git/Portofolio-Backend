using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonsController(ILessonService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LessonDto>>> GetAll([FromQuery] string lang = "id") =>
        Ok(await service.GetAllAsync(lang));

    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<List<LessonAdminDto>>> GetAllAdmin() => Ok(await service.GetAllAdminAsync());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<LessonAdminDto>> Create(LessonUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, LessonUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
