using Portfolio.Api.Services;

namespace Portfolio.Api.Tests.Fakes;

/// <summary>Records what would've been emailed instead of actually sending anything.</summary>
public class FakeEmailSender : IEmailSender
{
    public string? LastSentCode { get; private set; }
    public string? LastSentTo { get; private set; }

    public Task SendOtpCodeAsync(string toEmail, string code)
    {
        LastSentTo = toEmail;
        LastSentCode = code;
        return Task.CompletedTask;
    }
}
