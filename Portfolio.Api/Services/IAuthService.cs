using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

// RawRefreshToken is null whenever Response.Challenge != None (2FA still
// pending) — only a completed login actually issues a refresh token.
public record AuthResult(LoginResponseDto Response, string? RawRefreshToken);

public interface IAuthService
{
    Task<AuthResult?> LoginAsync(LoginRequestDto dto);
    Task<AuthResult?> GoogleLoginAsync(GoogleLoginRequestDto dto);
    Task<AuthResult?> MicrosoftLoginAsync(MicrosoftLoginRequestDto dto);
    Task<AuthResult?> VerifyTotpLoginAsync(VerifyTotpLoginRequestDto dto);
    Task<AuthResult?> VerifyEmailOtpLoginAsync(VerifyEmailOtpLoginRequestDto dto);
    Task<TotpSetupResponseDto?> SetupTotpAsync(string username);
    Task<bool> EnableTotpAsync(string username, string code);
    Task<bool> ChangePasswordAsync(string username, ChangePasswordRequestDto dto);
}
