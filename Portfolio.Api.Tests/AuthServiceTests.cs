using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Services;
using Portfolio.Api.Tests.Fakes;

namespace Portfolio.Api.Tests;

public class AuthServiceTests
{
    private static AuthService CreateAuthService(FakeAdminUserRepository repo, FakeEmailSender emailSender) =>
        new(
            repo,
            new TotpService(),
            emailSender,
            Options.Create(new JwtOptions
            {
                Issuer = "test-issuer",
                Audience = "portofolio-admin",
                SigningKey = "unit-test-signing-key-at-least-32-characters",
                ExpiresInMinutes = 180,
            }),
            Options.Create(new GoogleOptions { ClientId = "test-google-client-id" }),
            Options.Create(new MicrosoftOptions { ClientId = "test-microsoft-client-id" }),
            new ConfigurationManager<OpenIdConnectConfiguration>(
                "https://login.microsoftonline.com/consumers/v2.0/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever()));

    private static AdminUser NewUser(string username = "admin", string password = "ChangeMe123!") => new()
    {
        Username = username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        Email = $"{username}@example.com",
    };

    // --- Password change: this directly guards IsPasswordStrongEnough, which is
    // kept in sync by hand with the frontend's checklist UI. If someone loosens
    // this rule without meaning to, this is what catches it. ---

    [Theory]
    [InlineData("short1A")]      // under 8 chars
    [InlineData("alllowercase1")] // no uppercase
    [InlineData("NoDigitsHere")]  // no digit
    public async Task ChangePasswordAsync_RejectsPasswordsThatDontMeetTheRules(string weakNewPassword)
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        var result = await service.ChangePasswordAsync("admin", new ChangePasswordRequestDto("ChangeMe123!", weakNewPassword));

        Assert.False(result);
    }

    [Fact]
    public async Task ChangePasswordAsync_RejectsWrongCurrentPassword()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        var result = await service.ChangePasswordAsync("admin", new ChangePasswordRequestDto("TotallyWrongPassword1", "BrandNewPassword1"));

        Assert.False(result);
    }

    [Fact]
    public async Task ChangePasswordAsync_SucceedsWithCorrectCurrentPasswordAndStrongNewPassword()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        var result = await service.ChangePasswordAsync("admin", new ChangePasswordRequestDto("ChangeMe123!", "BrandNewPassword1"));

        Assert.True(result);
        var user = await repo.GetByUsernameAsync("admin");
        Assert.True(BCrypt.Net.BCrypt.Verify("BrandNewPassword1", user!.PasswordHash));
    }

    // --- Account lockout: the actual defense against brute-forcing, tied to the
    // account rather than the caller's IP (see GetLockoutDuration's doc comment). ---

    [Fact]
    public async Task LoginAsync_WrongPassword_DoesNotLockOutBeforeThirdAttempt()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        await service.LoginAsync(new LoginRequestDto("admin", "wrong-password"));
        await service.LoginAsync(new LoginRequestDto("admin", "wrong-password"));

        // Still only 2 failed attempts — correct password should still work.
        var result = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));
        Assert.NotNull(result);
        Assert.Equal(LoginChallenge.EmailOtp, result!.Challenge);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_LocksAccountOnThirdAttempt()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        await service.LoginAsync(new LoginRequestDto("admin", "wrong-password"));
        await service.LoginAsync(new LoginRequestDto("admin", "wrong-password"));
        await service.LoginAsync(new LoginRequestDto("admin", "wrong-password")); // 3rd -> 30s lockout

        // Even the CORRECT password must now be rejected, because the account is locked.
        var result = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));
        Assert.Null(result);

        var user = await repo.GetByUsernameAsync("admin");
        Assert.NotNull(user!.LockedUntilUtc);
        Assert.True(user.LockedUntilUtc > DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_CorrectPassword_ResetsFailedAttemptCounter()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        await service.LoginAsync(new LoginRequestDto("admin", "wrong-password"));
        await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!")); // correct -> resets

        var user = await repo.GetByUsernameAsync("admin");
        Assert.Equal(0, user!.FailedLoginAttempts);
    }

    [Fact]
    public async Task LoginAsync_UnknownUsername_ReturnsNullWithoutThrowing()
    {
        var repo = new FakeAdminUserRepository();
        var service = CreateAuthService(repo, new FakeEmailSender());

        var result = await service.LoginAsync(new LoginRequestDto("nobody", "whatever"));

        Assert.Null(result);
    }

    // --- Email OTP verification ---

    [Fact]
    public async Task LoginAsync_CorrectPassword_SendsOtpAndReturnsPendingToken()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var emailSender = new FakeEmailSender();
        var service = CreateAuthService(repo, emailSender);

        var result = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));

        Assert.NotNull(result);
        Assert.Equal(LoginChallenge.EmailOtp, result!.Challenge);
        Assert.NotNull(result.PendingToken);
        Assert.Null(result.AccessToken); // not logged in yet — still needs the OTP step
        Assert.Equal("admin@example.com", emailSender.LastSentTo);
        Assert.NotNull(emailSender.LastSentCode);
    }

    [Fact]
    public async Task VerifyEmailOtpLoginAsync_CorrectCode_IssuesRealAccessToken()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var emailSender = new FakeEmailSender();
        var service = CreateAuthService(repo, emailSender);

        var login = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));
        var verify = await service.VerifyEmailOtpLoginAsync(new VerifyEmailOtpLoginRequestDto(login!.PendingToken!, emailSender.LastSentCode!));

        Assert.NotNull(verify);
        Assert.Equal(LoginChallenge.None, verify!.Challenge);
        Assert.NotNull(verify.AccessToken);
    }

    [Fact]
    public async Task VerifyEmailOtpLoginAsync_WrongCode_Fails()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var emailSender = new FakeEmailSender();
        var service = CreateAuthService(repo, emailSender);

        var login = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));
        var verify = await service.VerifyEmailOtpLoginAsync(new VerifyEmailOtpLoginRequestDto(login!.PendingToken!, "000000"));

        Assert.Null(verify);
    }

    [Fact]
    public async Task VerifyEmailOtpLoginAsync_CodeIsOneTimeUse()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var emailSender = new FakeEmailSender();
        var service = CreateAuthService(repo, emailSender);

        var login = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));
        var code = emailSender.LastSentCode!;

        var first = await service.VerifyEmailOtpLoginAsync(new VerifyEmailOtpLoginRequestDto(login!.PendingToken!, code));
        Assert.NotNull(first); // consumes it

        // The pending token itself is also single-purpose/short-lived, but even
        // ignoring that: the OTP hash was cleared after the first successful use.
        var replay = await service.VerifyEmailOtpLoginAsync(new VerifyEmailOtpLoginRequestDto(login.PendingToken!, code));
        Assert.Null(replay);
    }

    // --- The audience-scoping guarantee: a pending token minted for ONE challenge
    // type must be rejected by the OTHER challenge's verify endpoint, even though
    // both are technically valid, signed, unexpired JWTs. This is the actual
    // mechanism that makes "pending" tokens safe to hand to an unauthenticated caller. ---

    [Fact]
    public async Task VerifyTotpLoginAsync_RejectsAnEmailOtpPendingToken()
    {
        var repo = new FakeAdminUserRepository();
        repo.Add(NewUser());
        var service = CreateAuthService(repo, new FakeEmailSender());

        var login = await service.LoginAsync(new LoginRequestDto("admin", "ChangeMe123!"));
        Assert.Equal(LoginChallenge.EmailOtp, login!.Challenge);

        // Try to spend the email-otp pending token on the TOTP verify endpoint instead.
        var result = await service.VerifyTotpLoginAsync(new VerifyTotpLoginRequestDto(login.PendingToken!, "123456"));

        Assert.Null(result);
    }
}
