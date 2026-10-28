namespace Portfolio.Api.Services;

public record RefreshResult(string AccessToken, DateTime AccessTokenExpiresAtUtc, string RawRefreshToken);

public interface IRefreshTokenService
{
    Task<(string RawToken, DateTime ExpiresAtUtc)> IssueAsync(string username);
    Task<RefreshResult?> RefreshAsync(string rawToken);
    Task RevokeAsync(string rawToken);
}
