using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class SkillRepository(AppDbContext db) : ISkillRepository
{
    public Task<List<Skill>> GetAllAsync() =>
        db.Skills.AsNoTracking().OrderBy(s => s.SortOrder).ToListAsync();

    public async Task<Skill> AddAsync(Skill skill)
    {
        db.Skills.Add(skill);
        await db.SaveChangesAsync();
        return skill;
    }

    public async Task<bool> UpdateAsync(Skill skill)
    {
        db.Skills.Update(skill);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var skill = await db.Skills.FindAsync(id);
        if (skill is null) return false;
        db.Skills.Remove(skill);
        return await db.SaveChangesAsync() > 0;
    }
}
