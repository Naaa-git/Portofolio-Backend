using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Tests.Fakes;

/// <summary>
/// In-memory stand-in for the real EF Core repository — acts like a tiny fake
/// database so tests can drive multi-step flows (failed attempts piling up,
/// lockout kicking in) without needing a real Postgres instance.
/// </summary>
public class FakeAdminUserRepository : IAdminUserRepository
{
    private readonly Dictionary<string, AdminUser> _usersByUsername = new();

    public void Add(AdminUser user) => _usersByUsername[user.Username] = user;

    public Task<AdminUser?> GetByUsernameAsync(string username) =>
        Task.FromResult(_usersByUsername.GetValueOrDefault(username));

    public Task<AdminUser?> GetByEmailAsync(string email) =>
        Task.FromResult(_usersByUsername.Values.FirstOrDefault(u => u.Email == email));

    public Task<bool> SetTotpSecretAsync(string username, string secret)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.TotpSecret = secret;
        return Task.FromResult(true);
    }

    public Task<bool> EnableTotpAsync(string username)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.TotpEnabled = true;
        return Task.FromResult(true);
    }

    public Task<bool> SetEmailOtpAsync(string username, string codeHash, DateTime expiresAtUtc)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.EmailOtpCodeHash = codeHash;
        user.EmailOtpExpiresAtUtc = expiresAtUtc;
        return Task.FromResult(true);
    }

    public Task<bool> ClearEmailOtpAsync(string username)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.EmailOtpCodeHash = null;
        user.EmailOtpExpiresAtUtc = null;
        return Task.FromResult(true);
    }

    public Task<bool> UpdatePasswordHashAsync(string username, string newPasswordHash)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.PasswordHash = newPasswordHash;
        return Task.FromResult(true);
    }

    public Task<int> IncrementFailedLoginAttemptsAsync(string username)
    {
        var user = _usersByUsername[username];
        user.FailedLoginAttempts++;
        return Task.FromResult(user.FailedLoginAttempts);
    }

    public Task<bool> SetLockoutAsync(string username, DateTime lockedUntilUtc)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.LockedUntilUtc = lockedUntilUtc;
        return Task.FromResult(true);
    }

    public Task<bool> ResetFailedLoginAsync(string username)
    {
        if (!_usersByUsername.TryGetValue(username, out var user)) return Task.FromResult(false);
        user.FailedLoginAttempts = 0;
        user.LockedUntilUtc = null;
        return Task.FromResult(true);
    }
}
