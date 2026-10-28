using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    Task RevokeAsync(int id);
    Task RevokeAllForUserAsync(string username);
}
