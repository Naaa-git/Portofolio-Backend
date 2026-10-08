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
    public string CategoryId { get; set; } = default!;
    public string CategoryEn { get; set; } = default!;
    public List<string> TechStack { get; set; } = new();
    public string ImageUrl { get; set; } = default!;
    public string GithubUrl { get; set; } = default!;
    public string DemoUrl { get; set; } = default!;
    public bool Featured { get; set; }
}
