using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        modelBuilder.Entity<AdminUser>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
