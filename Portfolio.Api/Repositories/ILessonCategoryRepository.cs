using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface ILessonCategoryRepository
{
    Task<List<LessonCategory>> GetAllAsync();
    Task<LessonCategory> AddAsync(LessonCategory category);
    Task<bool> UpdateAsync(LessonCategory category);
    Task<bool> DeleteAsync(int id);
}
