using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class SocialLinkRepository(AppDbContext db) : ISocialLinkRepository
{
    public Task<List<SocialLink>> GetAllAsync() =>
        db.SocialLinks.AsNoTracking().ToListAsync();

    public async Task<SocialLink> AddAsync(SocialLink link)
    {
        db.SocialLinks.Add(link);
        await db.SaveChangesAsync();
        return link;
    }

    public async Task<bool> UpdateAsync(SocialLink link)
    {
        db.SocialLinks.Update(link);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var link = await db.SocialLinks.FindAsync(id);
        if (link is null) return false;
        db.SocialLinks.Remove(link);
        return await db.SaveChangesAsync() > 0;
    }
}
