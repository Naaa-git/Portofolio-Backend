using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface ILessonRepository
{
    Task<List<Lesson>> GetAllAsync();
    Task<Lesson> AddAsync(Lesson lesson);
    Task<bool> UpdateAsync(Lesson lesson);
    Task<bool> DeleteAsync(int id);
}
