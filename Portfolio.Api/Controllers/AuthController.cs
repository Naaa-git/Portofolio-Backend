using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("google")]
    public async Task<ActionResult<LoginResponseDto>> GoogleLogin(GoogleLoginRequestDto dto)
    {
        var result = await authService.GoogleLoginAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("microsoft")]
    public async Task<ActionResult<LoginResponseDto>> MicrosoftLogin(MicrosoftLoginRequestDto dto)
    {
        var result = await authService.MicrosoftLoginAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("login/verify-totp")]
    public async Task<ActionResult<LoginResponseDto>> VerifyTotpLogin(VerifyTotpLoginRequestDto dto)
    {
        var result = await authService.VerifyTotpLoginAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("login/verify-email-otp")]
    public async Task<ActionResult<LoginResponseDto>> VerifyEmailOtpLogin(VerifyEmailOtpLoginRequestDto dto)
    {
        var result = await authService.VerifyEmailOtpLoginAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("totp/setup")]
    [Authorize]
    public async Task<ActionResult<TotpSetupResponseDto>> SetupTotp()
    {
        var username = User.Identity!.Name!;
        var result = await authService.SetupTotpAsync(username);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("totp/enable")]
    [Authorize]
    public async Task<IActionResult> EnableTotp(TotpVerifyRequestDto dto)
    {
        var username = User.Identity!.Name!;
        var success = await authService.EnableTotpAsync(username, dto.Code);
        return success ? NoContent() : BadRequest(new { message = "Kode salah atau sudah kedaluwarsa." });
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto dto)
    {
        var username = User.Identity!.Name!;
        var success = await authService.ChangePasswordAsync(username, dto);
        return success
            ? NoContent()
            : BadRequest(new { message = "Password saat ini salah, atau password baru belum memenuhi aturan (min. 8 karakter, ada huruf besar, ada angka)." });
    }
}
