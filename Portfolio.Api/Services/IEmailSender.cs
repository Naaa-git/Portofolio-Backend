namespace Portfolio.Api.Services;

public interface IEmailSender
{
    Task SendOtpCodeAsync(string toEmail, string code);
}
