using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto?> GoogleLoginAsync(GoogleLoginRequestDto dto);
    Task<LoginResponseDto?> MicrosoftLoginAsync(MicrosoftLoginRequestDto dto);
    Task<LoginResponseDto?> VerifyTotpLoginAsync(VerifyTotpLoginRequestDto dto);
    Task<LoginResponseDto?> VerifyEmailOtpLoginAsync(VerifyEmailOtpLoginRequestDto dto);
    Task<TotpSetupResponseDto?> SetupTotpAsync(string username);
    Task<bool> EnableTotpAsync(string username, string code);
    Task<bool> ChangePasswordAsync(string username, ChangePasswordRequestDto dto);
}
