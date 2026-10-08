using System.Web;
using OtpNet;

namespace Portfolio.Api.Services;

public class TotpService : ITotpService
{
    private const string Issuer = "Portofolio Admin";

    public string GenerateSecret()
    {
        var key = KeyGeneration.GenerateRandomKey(20); // 160-bit key, the standard TOTP size
        return Base32Encoding.ToString(key);
    }

    public string GenerateQrCodeUri(string secret, string accountName)
    {
        var label = HttpUtility.UrlEncode($"{Issuer}:{accountName}");
        var issuer = HttpUtility.UrlEncode(Issuer);
        return $"otpauth://totp/{label}?secret={secret}&issuer={issuer}&digits=6&period=30";
    }

    public bool ValidateCode(string secret, string code)
    {
        var totp = new Totp(Base32Encoding.ToBytes(secret));
        // Accept the previous/next 30s window too, so a slightly out-of-sync
        // clock on the phone doesn't lock the user out.
        return totp.VerifyTotp(code, out _, new VerificationWindow(previous: 1, future: 1));
    }
}
