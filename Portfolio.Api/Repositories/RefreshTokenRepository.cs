using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public async Task<RefreshToken> AddAsync(RefreshToken token)
    {
        db.RefreshTokens.Add(token);
        await db.SaveChangesAsync();
        return token;
    }

    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash) =>
        db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

    public async Task RevokeAsync(int id)
    {
        var token = await db.RefreshTokens.FindAsync(id);
        if (token is null) return;
        token.RevokedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task RevokeAllForUserAsync(string username)
    {
        await db.RefreshTokens
            .Where(t => t.Username == username && t.RevokedAtUtc == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.RevokedAtUtc, DateTime.UtcNow));
    }
}
