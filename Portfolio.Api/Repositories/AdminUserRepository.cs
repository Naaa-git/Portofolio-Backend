using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class AdminUserRepository(AppDbContext db) : IAdminUserRepository
{
    public Task<AdminUser?> GetByUsernameAsync(string username) =>
        db.AdminUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

    public Task<AdminUser?> GetByEmailAsync(string email) =>
        db.AdminUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> SetTotpSecretAsync(string username, string secret)
    {
        var user = await db.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return false;

        user.TotpSecret = secret;
        user.TotpEnabled = false; // require a successful /enable confirmation before it's active
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> EnableTotpAsync(string username)
    {
        var user = await db.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return false;

        user.TotpEnabled = true;
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> SetEmailOtpAsync(string username, string codeHash, DateTime expiresAtUtc)
    {
        var user = await db.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return false;

        user.EmailOtpCodeHash = codeHash;
        user.EmailOtpExpiresAtUtc = expiresAtUtc;
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> ClearEmailOtpAsync(string username)
    {
        var user = await db.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return false;

        user.EmailOtpCodeHash = null;
        user.EmailOtpExpiresAtUtc = null;
        return await db.SaveChangesAsync() > 0;
    }
}
