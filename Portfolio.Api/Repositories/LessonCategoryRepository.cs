using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class LessonCategoryRepository(AppDbContext db) : ILessonCategoryRepository
{
    public Task<List<LessonCategory>> GetAllAsync() =>
        db.LessonCategories.AsNoTracking().OrderBy(c => c.SortOrder).ToListAsync();

    public async Task<LessonCategory> AddAsync(LessonCategory category)
    {
        db.LessonCategories.Add(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(LessonCategory category)
    {
        db.LessonCategories.Update(category);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await db.LessonCategories.FindAsync(id);
        if (category is null) return false;
        db.LessonCategories.Remove(category);
        // DeleteBehavior.Restrict on Lesson.CategoryId throws DbUpdateException
        // here if any Lesson still references this category — left to bubble
        // up, the controller turns it into a 409 so the admin knows to move
        // or delete those Lessons first, instead of a silent/confusing 500.
        return await db.SaveChangesAsync() > 0;
    }
}
