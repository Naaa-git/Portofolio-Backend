namespace Portfolio.Api.Models.Entities;

public class Experience
{
    public int Id { get; set; }
    public string Company { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Period { get; set; } = default!;
    public string Location { get; set; } = default!;
    public bool Current { get; set; }
    public List<string> Description { get; set; } = new();
    public List<string> Skills { get; set; } = new();
}
