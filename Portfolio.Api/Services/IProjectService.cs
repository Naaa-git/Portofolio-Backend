using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetBySlugAsync(string slug);
    Task<ProjectDto> CreateAsync(ProjectUpsertDto dto);
    Task<bool> UpdateAsync(int id, ProjectUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
