using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class ProfileRepository(AppDbContext db) : IProfileRepository
{
    public Task<Profile?> GetAsync() =>
        db.Profiles.AsNoTracking().FirstOrDefaultAsync();

    public async Task<Profile> UpsertAsync(Profile profile)
    {
        var existing = await db.Profiles.FirstOrDefaultAsync();
        if (existing is null)
        {
            db.Profiles.Add(profile);
        }
        else
        {
            db.Entry(existing).CurrentValues.SetValues(profile);
        }

        await db.SaveChangesAsync();
        return existing ?? profile;
    }
}
