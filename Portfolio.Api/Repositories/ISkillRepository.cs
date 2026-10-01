using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface ISkillRepository
{
    Task<List<Skill>> GetAllAsync();
    Task<Skill> AddAsync(Skill skill);
    Task<bool> UpdateAsync(Skill skill);
    Task<bool> DeleteAsync(int id);
}
