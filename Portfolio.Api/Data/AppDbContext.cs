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

    public DbSet<OutsideCodeIntro> OutsideCodeIntros => Set<OutsideCodeIntro>();
    public DbSet<AwayFromKeyboardItem> AwayFromKeyboardItems => Set<AwayFromKeyboardItem>();
    public DbSet<MovieTake> MovieTakes => Set<MovieTake>();
    public DbSet<MusicArtist> MusicArtists => Set<MusicArtist>();
    public DbSet<PodcastChannel> PodcastChannels => Set<PodcastChannel>();
    public DbSet<OutsideCodeBook> OutsideCodeBooks => Set<OutsideCodeBook>();
    public DbSet<LifeInspiration> LifeInspirations => Set<LifeInspiration>();

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
