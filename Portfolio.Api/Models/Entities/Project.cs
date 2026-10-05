namespace Portfolio.Api.Models.Entities;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public Dictionary<string, string> ShortDescription { get; set; } = new();
    public Dictionary<string, string> LongDescription { get; set; } = new();
    public List<string> TechStack { get; set; } = new();
    public string ImageUrl { get; set; } = default!;
    public string GithubUrl { get; set; } = default!;
    public string DemoUrl { get; set; } = default!;
    public bool Featured { get; set; }
    public Dictionary<string, string> Category { get; set; } = new();
}
