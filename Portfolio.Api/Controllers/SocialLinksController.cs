using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/social-links")]
public class SocialLinksController(ISocialLinkService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SocialLinkDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SocialLinkDto>> Create(SocialLinkUpsertDto dto) => Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, SocialLinkUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
