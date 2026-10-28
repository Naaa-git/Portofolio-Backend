namespace Portfolio.Api.Models.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string TokenHash { get; set; } = default!;
    public string Username { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAtUtc { get; set; }
}
