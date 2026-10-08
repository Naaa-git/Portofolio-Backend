namespace Portfolio.Api.Services;

public interface ITotpService
{
    /// <summary>Generates a new random Base32 secret for a user setting up 2FA.</summary>
    string GenerateSecret();

    /// <summary>
    /// Builds the otpauth:// URI that, encoded as a QR code, lets an authenticator
    /// app (Google/Microsoft Authenticator, Authy, ...) import the secret.
    /// </summary>
    string GenerateQrCodeUri(string secret, string accountName);

    /// <summary>Checks whether the 6-digit code matches what the secret should produce right now.</summary>
    bool ValidateCode(string secret, string code);
}
