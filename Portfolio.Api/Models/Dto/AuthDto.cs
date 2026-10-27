using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Dto;

public record LoginRequestDto(
    [Required, StringLength(50)] string Username,
    [Required, StringLength(100)] string Password);

public enum LoginChallenge { None, Totp, EmailOtp }

/// <summary>
/// Shape varies by Challenge:
///   None     -> AccessToken + ExpiresAtUtc are set, login is complete.
///   Totp     -> PendingToken is set; client must call /auth/login/verify-totp next.
///   EmailOtp -> PendingToken is set; client must call /auth/login/verify-email-otp next.
/// </summary>
public record LoginResponseDto(LoginChallenge Challenge, string? AccessToken, DateTime? ExpiresAtUtc, string? PendingToken);

public record VerifyTotpLoginRequestDto(
    [Required] string PendingToken,
    [Required, RegularExpression(@"^\d{6}$")] string Code);

public record VerifyEmailOtpLoginRequestDto(
    [Required] string PendingToken,
    [Required, RegularExpression(@"^\d{6}$")] string Code);

public record GoogleLoginRequestDto([Required] string IdToken);

public record MicrosoftLoginRequestDto([Required] string IdToken);

public record ChangePasswordRequestDto(
    [Required] string CurrentPassword,
    [Required, StringLength(100, MinimumLength = 8)] string NewPassword);
