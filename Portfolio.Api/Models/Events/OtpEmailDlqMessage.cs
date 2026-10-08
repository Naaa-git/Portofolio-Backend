namespace Portfolio.Api.Models.Events;

// Deliberately excludes the OTP code — it's already useless by the time this
// is written (the send failed), and this topic is for investigation, not retry.
public record OtpEmailDlqMessage(string Email, string Error, DateTime FailedAtUtc);
