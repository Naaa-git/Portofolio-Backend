using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class AdminUserRepository(AppDbContext db) : IAdminUserRepository
{
    public Task<AdminUser?> GetByUsernameAsync(string username) =>
        db.AdminUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
}
