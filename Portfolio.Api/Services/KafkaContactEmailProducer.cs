using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Portfolio.Api.Models.Events;

namespace Portfolio.Api.Services;

public class KafkaContactEmailProducer : IContactEmailProducer, IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;
    private readonly string _dlqTopic;

    public KafkaContactEmailProducer(IOptions<KafkaOptions> options)
    {
        _topic = options.Value.ContactEmailTopic;
        _dlqTopic = options.Value.ContactEmailDlqTopic;
        _producer = new ProducerBuilder<Null, string>(new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        }).Build();
    }

    public async Task PublishAsync(ContactEmailMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
    }

    public async Task PublishToDlqAsync(ContactEmailDlqMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(_dlqTopic, new Message<Null, string> { Value = json });
    }

    public void Dispose() => _producer.Dispose();
}
