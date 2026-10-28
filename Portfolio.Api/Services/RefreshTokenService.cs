using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class RefreshTokenService(
    IRefreshTokenRepository repo,
    AccessTokenGenerator accessTokenGenerator,
    IOptions<JwtOptions> jwtOptions) : IRefreshTokenService
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public async Task<(string RawToken, DateTime ExpiresAtUtc)> IssueAsync(string username)
    {
        var rawToken = GenerateRawToken();
        var expiresAtUtc = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiresInDays);

        await repo.AddAsync(new RefreshToken
        {
            TokenHash = Hash(rawToken),
            Username = username,
            ExpiresAtUtc = expiresAtUtc,
        });

        return (rawToken, expiresAtUtc);
    }

    public async Task<RefreshResult?> RefreshAsync(string rawToken)
    {
        var existing = await repo.GetByTokenHashAsync(Hash(rawToken));
        if (existing is null) return null;
        if (existing.ExpiresAtUtc <= DateTime.UtcNow) return null;

        if (existing.RevokedAtUtc is not null)
        {
            var gracePeriod = TimeSpan.FromSeconds(_jwt.RefreshTokenGracePeriodSeconds);
            var withinGracePeriod = DateTime.UtcNow - existing.RevokedAtUtc.Value <= gracePeriod;

            if (!withinGracePeriod)
            {
                // Reused long after it was already rotated — looks like a stolen/replayed
                // token. Revoke everything for this user as a defensive measure; the
                // legitimate owner will just have to log in again.
                await repo.RevokeAllForUserAsync(existing.Username);
                return null;
            }
            // Within the grace period: most likely two tabs/requests refreshing at
            // nearly the same moment, not theft — fall through and rotate normally.
        }
        else
        {
            await repo.RevokeAsync(existing.Id);
        }

        var (accessToken, accessTokenExpiresAt) = accessTokenGenerator.Generate(
            existing.Username, _jwt.Audience, _jwt.ExpiresInMinutes);
        var (newRawRefreshToken, _) = await IssueAsync(existing.Username);

        return new RefreshResult(accessToken, accessTokenExpiresAt, newRawRefreshToken);
    }

    public async Task RevokeAsync(string rawToken)
    {
        var existing = await repo.GetByTokenHashAsync(Hash(rawToken));
        if (existing is null) return;
        await repo.RevokeAsync(existing.Id);
    }

    private static string GenerateRawToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

    private static string Hash(string rawToken) =>
        Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawToken)));
}
