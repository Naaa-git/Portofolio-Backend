namespace Portfolio.Api.Models.Entities;

public class Experience
{
    public int Id { get; set; }
    public string Company { get; set; } = default!;
    public Dictionary<string, string> Role { get; set; } = new();
    public string Type { get; set; } = default!;
    public string Period { get; set; } = default!;
    public string Location { get; set; } = default!;
    public bool Current { get; set; }
    public List<Dictionary<string, string>> Description { get; set; } = new();
    public List<string> Skills { get; set; } = new();
}
