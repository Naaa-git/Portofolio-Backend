using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface ILessonService
{
    Task<List<LessonDto>> GetAllAsync(string lang);
    Task<List<LessonAdminDto>> GetAllAdminAsync();
    Task<LessonAdminDto> CreateAsync(LessonUpsertDto dto);
    Task<bool> UpdateAsync(int id, LessonUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
