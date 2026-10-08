using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IExperienceService
{
    Task<List<ExperienceDto>> GetAllAsync(string lang);
    Task<List<ExperienceAdminDto>> GetAllAdminAsync();
    Task<ExperienceAdminDto> CreateAsync(ExperienceUpsertDto dto);
    Task<bool> UpdateAsync(int id, ExperienceUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
