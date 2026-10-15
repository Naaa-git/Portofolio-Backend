namespace Portfolio.Api.Models.Entities;

public enum ProjectCategory { Personal, Freelance, Work }

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public Dictionary<string, string> ShortDescription { get; set; } = new();
    public Dictionary<string, string> LongDescription { get; set; } = new();
    public List<string> TechStack { get; set; } = new();
    public string ImageUrl { get; set; } = default!;
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool Featured { get; set; }
    public ProjectCategory Category { get; set; }

    // Nullable — most projects (personal/freelance) aren't tied to any job.
    // Set exclusively via PUT /api/experiences/{id}/projects (Experience side
    // owns this relationship), never through the normal Project upsert.
    public int? ExperienceId { get; set; }
    public Experience? Experience { get; set; }
}
