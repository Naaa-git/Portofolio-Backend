using Portfolio.Api.Models.Events;

namespace Portfolio.Api.Services;

public interface IOtpEmailProducer
{
    Task PublishAsync(OtpEmailMessage message);
    Task PublishToDlqAsync(OtpEmailDlqMessage message);
}
