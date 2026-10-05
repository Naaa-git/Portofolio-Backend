namespace Portfolio.Api.Models.Entities;

// Grouped in one file (unlike Project/Skill/etc.) because these 6 types are
// all sub-content of a single page ("Outside Code"), not independent
// top-level resources — same reasoning as why they share one repository/
// service/controller instead of six each.

public class OutsideCodeIntro
{
    public int Id { get; set; }
    public Dictionary<string, string> Paragraph1 { get; set; } = new();
    public Dictionary<string, string> Paragraph2 { get; set; } = new();
}

public class AwayFromKeyboardItem
{
    public int Id { get; set; }
    public Dictionary<string, string> Title { get; set; } = new();
    public Dictionary<string, string> Note { get; set; } = new();
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class MovieTake
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public Dictionary<string, string> Take { get; set; } = new();
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
    public Dictionary<string, string> Note { get; set; } = new();
    public string? ImageUrl { get; set; }
    public bool IsCurrentlyReading { get; set; }
    public int SortOrder { get; set; }
}

public class LifeInspiration
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    /// <summary>What's taken from this person — e.g. "Knowledge", "Ambition".</summary>
    public Dictionary<string, string> Aspect { get; set; } = new();
    public Dictionary<string, string> Note { get; set; } = new();
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}
