namespace Portfolio.Api.Models.Entities;

public class LessonCategory
{
    public int Id { get; set; }
    public Dictionary<string, string> Name { get; set; } = new();
    public int SortOrder { get; set; }

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
