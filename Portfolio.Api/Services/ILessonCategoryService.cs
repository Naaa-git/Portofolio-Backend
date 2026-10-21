using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface ILessonCategoryService
{
    Task<List<LessonCategoryDto>> GetAllAsync(string lang);
    Task<List<LessonCategoryAdminDto>> GetAllAdminAsync();
    Task<LessonCategoryAdminDto> CreateAsync(LessonCategoryUpsertDto dto);
    Task<bool> UpdateAsync(int id, LessonCategoryUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
