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

    public async Task SendContactMessageAsync(string fromName, string fromEmail, string message)
    {
        httpClient.DefaultRequestHeaders.Remove("api-key");
        httpClient.DefaultRequestHeaders.Add("api-key", _options.ApiKey);

        var ownerEmail = string.IsNullOrWhiteSpace(_options.OwnerNotificationEmail)
            ? _options.SenderEmail
            : _options.OwnerNotificationEmail;

        var payload = new
        {
            sender = new { name = _options.SenderName, email = _options.SenderEmail },
            to = new[] { new { email = ownerEmail } },
            replyTo = new { name = fromName, email = fromEmail },
            subject = $"Pesan baru dari {fromName} (Contact Form)",
            htmlContent = $"""
                <p><strong>Nama:</strong> {fromName}</p>
                <p><strong>Email:</strong> {fromEmail}</p>
                <p><strong>Pesan:</strong></p>
                <p>{message}</p>
                """,
        };

        var response = await httpClient.PostAsJsonAsync("https://api.brevo.com/v3/smtp/email", payload);
        response.EnsureSuccessStatusCode();
    }
}
