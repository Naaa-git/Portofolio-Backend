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
    public async Task<ActionResult<List<ProjectDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{slug}")]
    public async Task<ActionResult<ProjectDto>> GetBySlug(string slug)
    {
        var project = await service.GetBySlugAsync(slug);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProjectDto>> Create(ProjectUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, ProjectUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
