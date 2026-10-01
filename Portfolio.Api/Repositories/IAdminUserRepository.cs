using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IAdminUserRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username);
    Task<AdminUser?> GetByEmailAsync(string email);
    Task<bool> SetTotpSecretAsync(string username, string secret);
    Task<bool> EnableTotpAsync(string username);
    Task<bool> SetEmailOtpAsync(string username, string codeHash, DateTime expiresAtUtc);
    Task<bool> ClearEmailOtpAsync(string username);
}
