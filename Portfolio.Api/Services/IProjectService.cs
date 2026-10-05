using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync(string lang);
    Task<ProjectDto?> GetBySlugAsync(string slug, string lang);
    Task<List<ProjectAdminDto>> GetAllAdminAsync();
    Task<ProjectAdminDto> CreateAsync(ProjectUpsertDto dto);
    Task<bool> UpdateAsync(int id, ProjectUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
