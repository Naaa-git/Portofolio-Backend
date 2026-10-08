using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController(IProjectService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll([FromQuery] string lang = "id") =>
        Ok(await service.GetAllAsync(lang));

    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<List<ProjectAdminDto>>> GetAllAdmin() => Ok(await service.GetAllAdminAsync());

    [HttpGet("search")]
    public async Task<ActionResult<List<ProjectDto>>> Search([FromQuery] string q, [FromQuery] string lang = "id") =>
        string.IsNullOrWhiteSpace(q) ? Ok(new List<ProjectDto>()) : Ok(await service.SearchAsync(q, lang));

    [HttpGet("{slug}")]
    public async Task<ActionResult<ProjectDto>> GetBySlug(string slug, [FromQuery] string lang = "id")
    {
        var project = await service.GetBySlugAsync(slug, lang);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProjectAdminDto>> Create(ProjectUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, ProjectUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
