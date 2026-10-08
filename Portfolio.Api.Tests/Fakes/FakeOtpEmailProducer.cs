using Portfolio.Api.Models.Events;
using Portfolio.Api.Services;

namespace Portfolio.Api.Tests.Fakes;

/// <summary>Records what would've been published to Kafka instead of actually publishing anything.</summary>
public class FakeOtpEmailProducer : IOtpEmailProducer
{
    public string? LastSentCode { get; private set; }
    public string? LastSentTo { get; private set; }

    public Task PublishAsync(OtpEmailMessage message)
    {
        LastSentTo = message.Email;
        LastSentCode = message.Code;
        return Task.CompletedTask;
    }

    public Task PublishToDlqAsync(OtpEmailDlqMessage message) => Task.CompletedTask;
}
