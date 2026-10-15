using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class ExperienceRepository(AppDbContext db) : IExperienceRepository
{
    public Task<List<Experience>> GetAllAsync() =>
        db.Experiences.AsNoTracking().Include(e => e.Projects)
            .OrderByDescending(e => e.Current).ThenByDescending(e => e.Id).ToListAsync();

    public async Task<Experience> AddAsync(Experience experience)
    {
        db.Experiences.Add(experience);
        await db.SaveChangesAsync();
        return experience;
    }

    public async Task<bool> UpdateAsync(Experience experience)
    {
        db.Experiences.Update(experience);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var experience = await db.Experiences.FindAsync(id);
        if (experience is null) return false;
        db.Experiences.Remove(experience);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> SetProjectLinksAsync(int experienceId, List<int> projectIds)
    {
        var affected = await db.Projects
            .Where(p => p.ExperienceId == experienceId || projectIds.Contains(p.Id))
            .ToListAsync();

        foreach (var project in affected)
            project.ExperienceId = projectIds.Contains(project.Id) ? experienceId : null;

        await db.SaveChangesAsync();
        return true;
    }
}
