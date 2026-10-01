namespace Portfolio.Api.Models.Entities;

public class SocialLink
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Icon { get; set; } = default!;
}
