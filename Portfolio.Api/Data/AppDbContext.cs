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

    public DbSet<LessonCategory> LessonCategories => Set<LessonCategory>();
    public DbSet<Lesson> Lessons => Set<Lesson>();

    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

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
            e.Property(x => x.Description).IsTranslatable();
        });

        // First FK/relation in this codebase. SetNull (not Cascade) — deleting
        // an Experience should unlink its projects, never delete them.
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Experience)
            .WithMany(e => e.Projects)
            .HasForeignKey(p => p.ExperienceId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Skill>(e => e.Property(x => x.Category).IsTranslatable());

        modelBuilder.Entity<Project>(e =>
        {
            e.Property(p => p.ShortDescription).IsTranslatable();
            e.Property(p => p.Contribution).IsTranslatableList();
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

        modelBuilder.Entity<LessonCategory>(e => e.Property(x => x.Name).IsTranslatable());

        modelBuilder.Entity<Lesson>(e =>
        {
            e.Property(x => x.Title).IsTranslatable();
            e.Property(x => x.Description).IsTranslatable();
        });

        // First required FK in this codebase — a Lesson must belong to a
        // category. Restrict (not Cascade): deleting a category that still
        // has Lessons should fail loudly, not silently wipe them.
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Category).WithMany(c => c.Lessons)
            .HasForeignKey(l => l.CategoryId).OnDelete(DeleteBehavior.Restrict);

        // Optional tags — same SetNull pattern as Project.ExperienceId.
        // No reverse nav collection: Project/Experience don't need to list
        // their tagged Lessons back for v1.
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Project).WithMany()
            .HasForeignKey(l => l.ProjectId).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Experience).WithMany()
            .HasForeignKey(l => l.ExperienceId).OnDelete(DeleteBehavior.SetNull);
    }
}
