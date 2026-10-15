namespace Portfolio.Api.Models.Search;

/// <summary>
/// Denormalized, search-optimized copy of a Project. Postgres stays the
/// source of truth; this document is what OpenSearch actually queries
/// against, and is kept in sync on every project write (sync-on-write).
/// Both languages are stored so a search still matches regardless of which
/// locale the visitor is currently browsing in.
/// </summary>
public class ProjectSearchDocument
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string ShortDescriptionId { get; set; } = default!;
    public string ShortDescriptionEn { get; set; } = default!;
    public string LongDescriptionId { get; set; } = default!;
    public string LongDescriptionEn { get; set; } = default!;
    // No longer per-language — Category is a fixed enum name now (e.g. "Work"),
    // same value regardless of locale.
    public string Category { get; set; } = default!;
    public List<string> TechStack { get; set; } = new();
    public string ImageUrl { get; set; } = default!;
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool Featured { get; set; }
    public string? ExperienceCompany { get; set; }
}
