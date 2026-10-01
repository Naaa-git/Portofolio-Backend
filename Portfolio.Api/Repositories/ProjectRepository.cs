using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class ProjectRepository(AppDbContext db) : IProjectRepository
{
    public Task<List<Project>> GetAllAsync() =>
        db.Projects.AsNoTracking().OrderByDescending(p => p.Featured).ThenByDescending(p => p.Id).ToListAsync();

    public Task<Project?> GetBySlugAsync(string slug) =>
        db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Slug == slug);

    public Task<Project?> GetByIdAsync(int id) =>
        db.Projects.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Project> AddAsync(Project project)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return project;
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        db.Projects.Update(project);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return false;
        db.Projects.Remove(project);
        return await db.SaveChangesAsync() > 0;
    }
}
