namespace Portfolio.Api.Models.Entities;

public class Experience
{
    public int Id { get; set; }
    public string Company { get; set; } = default!;
    public Dictionary<string, string> Role { get; set; } = new();
    public string Type { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    // Null means "present" — paired with Current below, not a separate signal.
    public DateOnly? EndDate { get; set; }
    public string Location { get; set; } = default!;
    public bool Current { get; set; }
    public Dictionary<string, string> Description { get; set; } = new();
    public List<string> Skills { get; set; } = new();

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
