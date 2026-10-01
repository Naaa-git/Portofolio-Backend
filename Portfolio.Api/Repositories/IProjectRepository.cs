using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetBySlugAsync(string slug);
    Task<Project?> GetByIdAsync(int id);
    Task<Project> AddAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
}
