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

    // Account-level brute-force defense: tracks wrong password/TOTP/email-OTP
    // attempts regardless of source IP, so an attacker can't bypass it by
    // rotating IPs — the counter is tied to the account being attacked, not
    // where the requests come from.
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockedUntilUtc { get; set; }
}
