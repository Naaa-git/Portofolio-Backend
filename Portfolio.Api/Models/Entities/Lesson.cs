namespace Portfolio.Api.Models.Entities;

public class Lesson
{
    public int Id { get; set; }

    public int CategoryId { get; set; }
    public LessonCategory Category { get; set; } = default!;

    public Dictionary<string, string> Title { get; set; } = new();
    public Dictionary<string, string> Description { get; set; } = new();

    // Both optional — a Lesson doesn't have to be tied to a specific
    // Project or Experience, it can just be a standalone insight.
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public int? ExperienceId { get; set; }
    public Experience? Experience { get; set; }
}
