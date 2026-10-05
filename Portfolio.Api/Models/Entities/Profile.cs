namespace Portfolio.Api.Models.Entities;

public class Profile
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ShortName { get; set; } = default!;
    public Dictionary<string, string> Role { get; set; } = new();
    public List<Dictionary<string, string>> RoleAlternatives { get; set; } = new();
    public Dictionary<string, string> Tagline { get; set; } = new();
    public Dictionary<string, string> Bio { get; set; } = new();
    public Dictionary<string, string> BioExtended { get; set; } = new();
    public bool AvailableForWork { get; set; }
    public string Email { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string AvatarUrl { get; set; } = default!;
    public string CvUrl { get; set; } = default!;
}
