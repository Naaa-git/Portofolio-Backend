using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;
using QRCoder;

namespace Portfolio.Api.Services;

public class AuthService(
    IAdminUserRepository repo,
    ITotpService totpService,
    IEmailSender emailSender,
    IOptions<JwtOptions> jwtOptions,
    IOptions<GoogleOptions> googleOptions,
    IOptions<MicrosoftOptions> microsoftOptions,
    ConfigurationManager<OpenIdConnectConfiguration> microsoftConfigManager) : IAuthService
{
    private readonly JwtOptions _jwt = jwtOptions.Value;
    private readonly GoogleOptions _google = googleOptions.Value;
    private readonly MicrosoftOptions _microsoft = microsoftOptions.Value;

    // The fixed, publicly-documented tenant ID Microsoft uses for personal
    // Microsoft accounts (outlook.com/hotmail.com/live.com) on the v2.0 endpoint.
    // Not a secret — it's the same for every app that only wants personal accounts.
    private const string MicrosoftConsumersIssuer = "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";

    // Each pending-token purpose gets its own audience, so a token minted for one
    // challenge (say, email OTP) is automatically useless for another (say, TOTP),
    // and useless for every normal [Authorize] endpoint — same trick as Fase 2.
    private const string TotpPendingAudience = "portofolio-admin-2fa-pending";
    private const string EmailOtpPendingAudience = "portofolio-admin-emailotp-pending";
    private const int PendingTokenExpiresInMinutes = 5;
    private const int EmailOtpExpiresInMinutes = 10;

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var user = await repo.GetByUsernameAsync(dto.Username);
        if (user is null) return null;

        if (IsLockedOut(user)) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            await RegisterFailedAttemptAsync(user.Username, "password");
            return null;
        }

        await repo.ResetFailedLoginAsync(user.Username);

        // Path C (password) is paired with Email OTP, not TOTP — by design,
        // independent of whatever user.TotpEnabled is set to (that's for the
        // OAuth paths only).
        return await IssueEmailOtpChallengeAsync(user);
    }

    public async Task<LoginResponseDto?> GoogleLoginAsync(GoogleLoginRequestDto dto)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_google.ClientId],
            });
        }
        catch (InvalidJwtException)
        {
            // Token wasn't actually signed by Google, or isn't meant for our Client ID.
            return null;
        }

        if (!payload.EmailVerified) return null;

        // This is the allowlist check: proving "this is really this email" (Google's job)
        // is not the same as "this email is allowed to log in" (our job).
        var user = await repo.GetByEmailAsync(payload.Email);
        if (user is null) return null;

        return IssueTotpChallengeResult(user);
    }

    public async Task<LoginResponseDto?> MicrosoftLoginAsync(MicrosoftLoginRequestDto dto)
    {
        var email = await ValidateMicrosoftIdTokenAsync(dto.IdToken);
        if (email is null) return null;

        // Same allowlist check as Google: Microsoft proved "this is really this
        // email", but whether that email is allowed in is our decision, not theirs.
        var user = await repo.GetByEmailAsync(email);
        if (user is null) return null;

        return IssueTotpChallengeResult(user);
    }

    public async Task<LoginResponseDto?> VerifyTotpLoginAsync(VerifyTotpLoginRequestDto dto)
    {
        var username = ValidatePendingToken(dto.PendingToken, TotpPendingAudience);
        if (username is null) return null;

        var user = await repo.GetByUsernameAsync(username);
        if (user is null) return null;

        if (IsLockedOut(user)) return null;

        if (user.TotpSecret is null || !totpService.ValidateCode(user.TotpSecret, dto.Code))
        {
            await RegisterFailedAttemptAsync(username, "totp");
            return null;
        }

        await repo.ResetFailedLoginAsync(username);

        var (accessToken, expiresAt) = GenerateToken(user.Username, _jwt.Audience, _jwt.ExpiresInMinutes);
        return new LoginResponseDto(LoginChallenge.None, accessToken, expiresAt, null);
    }

    public async Task<LoginResponseDto?> VerifyEmailOtpLoginAsync(VerifyEmailOtpLoginRequestDto dto)
    {
        var username = ValidatePendingToken(dto.PendingToken, EmailOtpPendingAudience);
        if (username is null) return null;

        var user = await repo.GetByUsernameAsync(username);
        if (user is null) return null;

        if (IsLockedOut(user)) return null;

        var codeIsValid = user.EmailOtpCodeHash is not null
            && user.EmailOtpExpiresAtUtc is not null
            && user.EmailOtpExpiresAtUtc >= DateTime.UtcNow
            && HashOtpCode(dto.Code) == user.EmailOtpCodeHash;

        if (!codeIsValid)
        {
            await RegisterFailedAttemptAsync(username, "email_otp");
            return null;
        }

        await repo.ResetFailedLoginAsync(username);
        await repo.ClearEmailOtpAsync(username); // one-time use — consume it immediately

        var (accessToken, expiresAt) = GenerateToken(user.Username, _jwt.Audience, _jwt.ExpiresInMinutes);
        return new LoginResponseDto(LoginChallenge.None, accessToken, expiresAt, null);
    }

    public async Task<TotpSetupResponseDto?> SetupTotpAsync(string username)
    {
        var user = await repo.GetByUsernameAsync(username);
        if (user is null) return null;

        var secret = totpService.GenerateSecret();
        await repo.SetTotpSecretAsync(username, secret);

        var otpauthUri = totpService.GenerateQrCodeUri(secret, user.Username);

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(otpauthUri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(10);

        return new TotpSetupResponseDto(secret, Convert.ToBase64String(qrCodeBytes));
    }

    public async Task<bool> EnableTotpAsync(string username, string code)
    {
        var user = await repo.GetByUsernameAsync(username);
        if (user?.TotpSecret is null) return false;

        if (!totpService.ValidateCode(user.TotpSecret, code)) return false;

        return await repo.EnableTotpAsync(username);
    }

    public async Task<bool> ChangePasswordAsync(string username, ChangePasswordRequestDto dto)
    {
        if (!IsPasswordStrongEnough(dto.NewPassword)) return false;

        var user = await repo.GetByUsernameAsync(username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            return false;

        var newHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        return await repo.UpdatePasswordHashAsync(username, newHash);
    }

    private async Task<LoginResponseDto?> IssueEmailOtpChallengeAsync(AdminUser user)
    {
        if (string.IsNullOrEmpty(user.Email)) return null; // nowhere to send the code

        var code = GenerateNumericCode(6);

        try
        {
            await emailSender.SendOtpCodeAsync(user.Email, code);
        }
        catch (HttpRequestException)
        {
            // Don't hand out a pending token for a code that was never actually
            // delivered — that would leave the user stuck with no way forward.
            return null;
        }

        await repo.SetEmailOtpAsync(user.Username, HashOtpCode(code), DateTime.UtcNow.AddMinutes(EmailOtpExpiresInMinutes));

        var (pendingToken, _) = GenerateToken(user.Username, EmailOtpPendingAudience, PendingTokenExpiresInMinutes);
        return new LoginResponseDto(LoginChallenge.EmailOtp, null, null, pendingToken);
    }

    private LoginResponseDto IssueTotpChallengeResult(AdminUser user)
    {
        if (!user.TotpEnabled)
        {
            var (accessToken, expiresAt) = GenerateToken(user.Username, _jwt.Audience, _jwt.ExpiresInMinutes);
            return new LoginResponseDto(LoginChallenge.None, accessToken, expiresAt, null);
        }

        var (pendingToken, _) = GenerateToken(user.Username, TotpPendingAudience, PendingTokenExpiresInMinutes);
        return new LoginResponseDto(LoginChallenge.Totp, null, null, pendingToken);
    }

    // Kept in sync with the checklist shown in the frontend form — if you add a
    // rule here, add it there too (and vice versa), or the UI will lie about
    // what the server actually requires.
    private static bool IsPasswordStrongEnough(string password) =>
        password.Length >= 8
        && password.Any(char.IsUpper)
        && password.Any(char.IsDigit);

    private static bool IsLockedOut(AdminUser user) =>
        user.LockedUntilUtc is not null && user.LockedUntilUtc > DateTime.UtcNow;

    private async Task RegisterFailedAttemptAsync(string username, string stage)
    {
        AppMetrics.FailedLoginAttempts.WithLabels(stage).Inc();

        var attempts = await repo.IncrementFailedLoginAttemptsAsync(username);
        var lockoutDuration = GetLockoutDuration(attempts);
        if (lockoutDuration > TimeSpan.Zero)
        {
            await repo.SetLockoutAsync(username, DateTime.UtcNow.Add(lockoutDuration));
            AppMetrics.AccountLockouts.Inc();
        }
    }

    /// <summary>
    /// Progressive backoff tied to the ACCOUNT, not the source IP — the first couple
    /// of mistakes (fat-fingered password, mistyped code) go unpunished, but repeated
    /// failures from any IP (or many rotating IPs, as in a botnet attack) escalate the
    /// same counter, since they're all attacking the one account that matters here.
    /// </summary>
    private static TimeSpan GetLockoutDuration(int failedAttempts) => failedAttempts switch
    {
        <= 2 => TimeSpan.Zero,
        3 => TimeSpan.FromSeconds(30),
        4 => TimeSpan.FromMinutes(2),
        5 => TimeSpan.FromMinutes(5),
        6 => TimeSpan.FromMinutes(15),
        _ => TimeSpan.FromMinutes(30),
    };

    private static string GenerateNumericCode(int digits)
    {
        var max = (int)Math.Pow(10, digits);
        var value = RandomNumberGenerator.GetInt32(0, max); // cryptographically secure, not Random
        return value.ToString(new string('0', digits));
    }

    private static string HashOtpCode(string code) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(code)));

    private async Task<string?> ValidateMicrosoftIdTokenAsync(string idToken)
    {
        // Unlike Google.Apis.Auth (which bundles everything), here we fetch
        // Microsoft's current public signing keys ourselves, then validate
        // the token manually with the same JwtSecurityTokenHandler our own
        // tokens use — it's the same mechanism, just pointed at someone else's keys.
        OpenIdConnectConfiguration config;
        try
        {
            config = await microsoftConfigManager.GetConfigurationAsync();
        }
        catch (Exception)
        {
            return null; // couldn't reach Microsoft's metadata endpoint
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = MicrosoftConsumersIssuer,
            ValidateAudience = true,
            ValidAudience = _microsoft.ClientId,
            ValidateLifetime = true,
            IssuerSigningKeys = config.SigningKeys,
        };

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(idToken, validationParameters, out _);
            return principal.FindFirst("email")?.Value ?? principal.FindFirst("preferred_username")?.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private (string Token, DateTime ExpiresAt) GenerateToken(string username, string audience, int expiresInMinutes)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Name, username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    private string? ValidatePendingToken(string pendingToken, string expectedAudience)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwt.Issuer,
            ValidAudience = expectedAudience, // only tokens minted for THIS exact purpose pass
            IssuerSigningKey = key,
        };

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(pendingToken, validationParameters, out _);
            return principal.Identity?.Name;
        }
        catch (Exception)
        {
            // Any failure here (expired, wrong audience, wrong signature, or even a
            // string that isn't shaped like a JWT at all) just means "not valid for
            // this purpose" — always treated as a rejected login, never a server error.
            return null;
        }
    }
}
