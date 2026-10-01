namespace Portfolio.Api.Models.Entities;

public class Profile
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ShortName { get; set; } = default!;
    public string Role { get; set; } = default!;
    public List<string> RoleAlternatives { get; set; } = new();
    public string Tagline { get; set; } = default!;
    public string Bio { get; set; } = default!;
    public string BioExtended { get; set; } = default!;
    public bool AvailableForWork { get; set; }
    public string Email { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string AvatarUrl { get; set; } = default!;
    public string CvUrl { get; set; } = default!;
}
