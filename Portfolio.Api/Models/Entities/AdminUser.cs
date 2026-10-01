namespace Portfolio.Api.Models.Entities;

public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
}
