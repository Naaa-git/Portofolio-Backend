using Portfolio.Api.Models.Events;

namespace Portfolio.Api.Services;

public interface IContactEmailProducer
{
    Task PublishAsync(ContactEmailMessage message);
    Task PublishToDlqAsync(ContactEmailDlqMessage message);
}
