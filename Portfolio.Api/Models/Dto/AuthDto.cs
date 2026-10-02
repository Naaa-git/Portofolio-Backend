namespace Portfolio.Api.Models.Dto;

public record LoginRequestDto(string Username, string Password);

public enum LoginChallenge { None, Totp, EmailOtp }

/// <summary>
/// Shape varies by Challenge:
///   None     -> AccessToken + ExpiresAtUtc are set, login is complete.
///   Totp     -> PendingToken is set; client must call /auth/login/verify-totp next.
///   EmailOtp -> PendingToken is set; client must call /auth/login/verify-email-otp next.
/// </summary>
public record LoginResponseDto(LoginChallenge Challenge, string? AccessToken, DateTime? ExpiresAtUtc, string? PendingToken);

public record VerifyTotpLoginRequestDto(string PendingToken, string Code);

public record VerifyEmailOtpLoginRequestDto(string PendingToken, string Code);

public record GoogleLoginRequestDto(string IdToken);

public record MicrosoftLoginRequestDto(string IdToken);

public record ChangePasswordRequestDto(string CurrentPassword, string NewPassword);
