namespace Portfolio.Api.Models.Entities;

public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    /// <summary>
    /// The one email allowed to log in via Google/Microsoft OAuth for this account.
    /// This is the allowlist check — OAuth only proves "this really is this email",
    /// it's this field that decides "and that email is allowed in".
    /// </summary>
    public string? Email { get; set; }

    public string? TotpSecret { get; set; }
    public bool TotpEnabled { get; set; }

    // Email OTP is one-shot: generated at login time, consumed (cleared) right
    // after a successful verify, so there's never a "stale valid code" lying around.
    public string? EmailOtpCodeHash { get; set; }
    public DateTime? EmailOtpExpiresAtUtc { get; set; }
}
