namespace Portfolio.Api.Models.Entities;

public class Skill
{
    public int Id { get; set; }
    public string Category { get; set; } = default!;
    public List<string> Items { get; set; } = new();
    public int SortOrder { get; set; }
}
