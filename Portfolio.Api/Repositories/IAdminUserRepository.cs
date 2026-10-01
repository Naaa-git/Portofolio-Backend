using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IAdminUserRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username);
}
