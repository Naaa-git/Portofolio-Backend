using Portfolio.Api.Services;

namespace Portfolio.Api.Tests.Fakes;

// AuthServiceTests only cares that a raw refresh token gets handed back
// alongside a completed login — the rotation/grace-period logic itself is
// RefreshTokenService's own concern (not exercised here).
public class FakeRefreshTokenService : IRefreshTokenService
{
    public Task<(string RawToken, DateTime ExpiresAtUtc)> IssueAsync(string username) =>
        Task.FromResult(($"fake-refresh-token-for-{username}", DateTime.UtcNow.AddDays(30)));

    public Task<RefreshResult?> RefreshAsync(string rawToken) => Task.FromResult<RefreshResult?>(null);

    public Task RevokeAsync(string rawToken) => Task.CompletedTask;
}
