namespace Portfolio.Api.Services;

public interface IEmailSender
{
    Task SendOtpCodeAsync(string toEmail, string code);
    Task SendContactMessageAsync(string fromName, string fromEmail, string message);
}
