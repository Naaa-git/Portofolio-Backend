using OtpNet;
using Portfolio.Api.Services;

namespace Portfolio.Api.Tests;

public class TotpServiceTests
{
    [Fact]
    public void GenerateSecret_ProducesADifferentSecretEachTime()
    {
        var service = new TotpService();

        var first = service.GenerateSecret();
        var second = service.GenerateSecret();

        Assert.NotEqual(first, second);
        // Must be valid Base32 (what authenticator apps expect) — this throws if not.
        Base32Encoding.ToBytes(first);
    }

    [Fact]
    public void ValidateCode_AcceptsTheCodeAnAuthenticatorAppWouldGenerateRightNow()
    {
        var service = new TotpService();
        var secret = service.GenerateSecret();

        // Stand in for "what Google Authenticator would show on screen right now" —
        // computed independently via OtpNet's own Totp class against the same secret.
        var currentCode = new Totp(Base32Encoding.ToBytes(secret)).ComputeTotp();

        Assert.True(service.ValidateCode(secret, currentCode));
    }

    [Fact]
    public void ValidateCode_RejectsAnIncorrectCode()
    {
        var service = new TotpService();
        var secret = service.GenerateSecret();
        var realCode = new Totp(Base32Encoding.ToBytes(secret)).ComputeTotp();

        // Flip the code so it's guaranteed not to be the valid one.
        var wrongCode = realCode == "000000" ? "111111" : "000000";

        Assert.False(service.ValidateCode(secret, wrongCode));
    }

    [Fact]
    public void ValidateCode_RejectsACodeGeneratedFromADifferentSecret()
    {
        var service = new TotpService();
        var secretA = service.GenerateSecret();
        var secretB = service.GenerateSecret();
        var codeForB = new Totp(Base32Encoding.ToBytes(secretB)).ComputeTotp();

        Assert.False(service.ValidateCode(secretA, codeForB));
    }

    [Fact]
    public void GenerateQrCodeUri_ProducesAnOtpauthUriContainingTheSecret()
    {
        var service = new TotpService();
        var secret = service.GenerateSecret();

        var uri = service.GenerateQrCodeUri(secret, "admin");

        Assert.StartsWith("otpauth://totp/", uri);
        Assert.Contains($"secret={secret}", uri);
    }
}
