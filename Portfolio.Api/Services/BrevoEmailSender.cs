using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace Portfolio.Api.Services;

public class BrevoEmailSender(HttpClient httpClient, IOptions<BrevoOptions> options) : IEmailSender
{
    private readonly BrevoOptions _options = options.Value;

    public async Task SendOtpCodeAsync(string toEmail, string code)
    {
        httpClient.DefaultRequestHeaders.Remove("api-key");
        httpClient.DefaultRequestHeaders.Add("api-key", _options.ApiKey);

        var payload = new
        {
            sender = new { name = _options.SenderName, email = _options.SenderEmail },
            to = new[] { new { email = toEmail } },
            subject = "Kode login Portfolio Admin",
            htmlContent = $"""
                <p>Kode login kamu: <strong style="font-size: 20px">{code}</strong></p>
                <p>Berlaku 10 menit. Abaikan email ini kalau kamu nggak merasa mencoba login.</p>
                """,
        };

        var response = await httpClient.PostAsJsonAsync("https://api.brevo.com/v3/smtp/email", payload);
        response.EnsureSuccessStatusCode();
    }
}
