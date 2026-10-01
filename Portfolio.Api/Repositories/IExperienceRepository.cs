using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IExperienceRepository
{
    Task<List<Experience>> GetAllAsync();
    Task<Experience> AddAsync(Experience experience);
    Task<bool> UpdateAsync(Experience experience);
    Task<bool> DeleteAsync(int id);
}
