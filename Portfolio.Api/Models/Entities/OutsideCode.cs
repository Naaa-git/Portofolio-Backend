namespace Portfolio.Api.Models.Entities;

// Grouped in one file (unlike Project/Skill/etc.) because these 6 types are
// all sub-content of a single page ("Outside Code"), not independent
// top-level resources — same reasoning as why they share one repository/
// service/controller instead of six each.

public class OutsideCodeIntro
{
    public int Id { get; set; }
    public string Paragraph1 { get; set; } = default!;
    public string Paragraph2 { get; set; } = default!;
}

public class AwayFromKeyboardItem
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Note { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class MovieTake
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Take { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class MusicArtist
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class PodcastChannel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class OutsideCodeBook
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string? Note { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsCurrentlyReading { get; set; }
    public int SortOrder { get; set; }
}

public class LifeInspiration
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    /// <summary>What's taken from this person — e.g. "Knowledge", "Ambition".</summary>
    public string Aspect { get; set; } = default!;
    public string Note { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}
