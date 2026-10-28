using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("auth")]
public class AuthController(
    IAuthService authService,
    IRefreshTokenService refreshTokenService,
    IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    private const string RefreshCookieName = "refresh_token";
    private readonly JwtOptions _jwt = jwtOptions.Value;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return RespondWithAuthResult(result);
    }

    [HttpPost("google")]
    public async Task<ActionResult<LoginResponseDto>> GoogleLogin(GoogleLoginRequestDto dto)
    {
        var result = await authService.GoogleLoginAsync(dto);
        return RespondWithAuthResult(result);
    }

    [HttpPost("microsoft")]
    public async Task<ActionResult<LoginResponseDto>> MicrosoftLogin(MicrosoftLoginRequestDto dto)
    {
        var result = await authService.MicrosoftLoginAsync(dto);
        return RespondWithAuthResult(result);
    }

    [HttpPost("login/verify-totp")]
    public async Task<ActionResult<LoginResponseDto>> VerifyTotpLogin(VerifyTotpLoginRequestDto dto)
    {
        var result = await authService.VerifyTotpLoginAsync(dto);
        return RespondWithAuthResult(result);
    }

    [HttpPost("login/verify-email-otp")]
    public async Task<ActionResult<LoginResponseDto>> VerifyEmailOtpLogin(VerifyEmailOtpLoginRequestDto dto)
    {
        var result = await authService.VerifyEmailOtpLoginAsync(dto);
        return RespondWithAuthResult(result);
    }

    // Called automatically by the frontend when an access token expires — relies
    // on the HttpOnly refresh_token cookie, not on the (expired) Authorization
    // header, so this deliberately has no [Authorize].
    [HttpPost("refresh")]
    [EnableRateLimiting("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue(RefreshCookieName, out var rawToken))
            return Unauthorized();

        var result = await refreshTokenService.RefreshAsync(rawToken);
        if (result is null)
        {
            Response.Cookies.Delete(RefreshCookieName, new CookieOptions { Path = "/api/auth" });
            return Unauthorized();
        }

        SetRefreshCookie(result.RawRefreshToken);
        return Ok(new { accessToken = result.AccessToken, expiresAtUtc = result.AccessTokenExpiresAtUtc });
    }

    // No [Authorize] — logout must work even when the access token has already
    // expired, which is the most common reason someone hits this endpoint.
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue(RefreshCookieName, out var rawToken))
            await refreshTokenService.RevokeAsync(rawToken);

        Response.Cookies.Delete(RefreshCookieName, new CookieOptions { Path = "/api/auth" });
        return NoContent();
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

    private ActionResult<LoginResponseDto> RespondWithAuthResult(Services.AuthResult? result)
    {
        if (result is null) return Unauthorized();
        if (result.RawRefreshToken is not null) SetRefreshCookie(result.RawRefreshToken);
        return Ok(result.Response);
    }

    private void SetRefreshCookie(string rawToken)
    {
        Response.Cookies.Append(RefreshCookieName, rawToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/auth",
            Expires = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenExpiresInDays),
        });
    }
}
