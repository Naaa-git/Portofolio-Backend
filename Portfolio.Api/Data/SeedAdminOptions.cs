namespace Portfolio.Api.Data;

public class SeedAdminOptions
{
    public const string SectionName = "SeedAdmin";

    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
}
