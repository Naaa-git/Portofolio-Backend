using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IExperienceService
{
    Task<List<ExperienceDto>> GetAllAsync();
    Task<ExperienceDto> CreateAsync(ExperienceUpsertDto dto);
    Task<bool> UpdateAsync(int id, ExperienceUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
