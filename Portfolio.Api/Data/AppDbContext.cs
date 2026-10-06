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
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

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

        modelBuilder.Entity<Profile>(e =>
        {
            e.Property(p => p.Role).IsTranslatable();
            e.Property(p => p.RoleAlternatives).IsTranslatableList();
            e.Property(p => p.Tagline).IsTranslatable();
            e.Property(p => p.Bio).IsTranslatable();
            e.Property(p => p.BioExtended).IsTranslatable();
        });

        modelBuilder.Entity<Experience>(e =>
        {
            e.Property(x => x.Role).IsTranslatable();
            e.Property(x => x.Description).IsTranslatableList();
        });

        modelBuilder.Entity<Skill>(e => e.Property(x => x.Category).IsTranslatable());

        modelBuilder.Entity<Project>(e =>
        {
            e.Property(p => p.ShortDescription).IsTranslatable();
            e.Property(p => p.LongDescription).IsTranslatable();
            e.Property(p => p.Category).IsTranslatable();
        });

        modelBuilder.Entity<OutsideCodeIntro>(e =>
        {
            e.Property(x => x.Paragraph1).IsTranslatable();
            e.Property(x => x.Paragraph2).IsTranslatable();
        });

        modelBuilder.Entity<AwayFromKeyboardItem>(e =>
        {
            e.Property(x => x.Title).IsTranslatable();
            e.Property(x => x.Note).IsTranslatable();
        });

        modelBuilder.Entity<MovieTake>(e => e.Property(x => x.Take).IsTranslatable());

        modelBuilder.Entity<OutsideCodeBook>(e => e.Property(x => x.Note).IsTranslatable());

        modelBuilder.Entity<LifeInspiration>(e =>
        {
            e.Property(x => x.Aspect).IsTranslatable();
            e.Property(x => x.Note).IsTranslatable();
        });
    }
}
