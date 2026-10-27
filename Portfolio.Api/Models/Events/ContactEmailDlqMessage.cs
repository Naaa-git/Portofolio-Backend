namespace Portfolio.Api.Models.Events;

public record ContactEmailDlqMessage(int ContactMessageId, string Error, DateTime FailedAtUtc);
