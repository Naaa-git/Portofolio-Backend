namespace Portfolio.Api.Models.Entities;

public class Skill
{
    public int Id { get; set; }
    public Dictionary<string, string> Category { get; set; } = new();
    public List<string> Items { get; set; } = new();
    public int SortOrder { get; set; }
}
