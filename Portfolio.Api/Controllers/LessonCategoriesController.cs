using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/lesson-categories")]
public class LessonCategoriesController(ILessonCategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LessonCategoryDto>>> GetAll([FromQuery] string lang = "id") =>
        Ok(await service.GetAllAsync(lang));

    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<List<LessonCategoryAdminDto>>> GetAllAdmin() =>
        Ok(await service.GetAllAdminAsync());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<LessonCategoryAdminDto>> Create(LessonCategoryUpsertDto dto) =>
        Ok(await service.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, LessonCategoryUpsertDto dto) =>
        await service.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await service.DeleteAsync(id) ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            // DeleteBehavior.Restrict on Lesson.CategoryId — this category
            // still has Lessons pointing at it, move/delete those first.
            return Conflict(new { message = "Kategori ini masih dipakai oleh Lesson lain. Pindahkan atau hapus Lesson-nya dulu." });
        }
    }
}
