using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class LessonRepository(AppDbContext db) : ILessonRepository
{
    public Task<List<Lesson>> GetAllAsync() =>
        db.Lessons.AsNoTracking()
            .Include(l => l.Category)
            .Include(l => l.Project)
            .Include(l => l.Experience)
            .OrderBy(l => l.Id)
            .ToListAsync();

    public async Task<Lesson> AddAsync(Lesson lesson)
    {
        db.Lessons.Add(lesson);
        await db.SaveChangesAsync();
        return lesson;
    }

    public async Task<bool> UpdateAsync(Lesson lesson)
    {
        db.Lessons.Update(lesson);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lesson = await db.Lessons.FindAsync(id);
        if (lesson is null) return false;
        db.Lessons.Remove(lesson);
        return await db.SaveChangesAsync() > 0;
    }
}
