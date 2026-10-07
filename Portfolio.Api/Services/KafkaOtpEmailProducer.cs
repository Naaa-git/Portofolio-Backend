using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Portfolio.Api.Models.Events;

namespace Portfolio.Api.Services;

public class KafkaOtpEmailProducer : IOtpEmailProducer, IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaOtpEmailProducer(IOptions<KafkaOptions> options)
    {
        _topic = options.Value.OtpEmailTopic;
        _producer = new ProducerBuilder<Null, string>(new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        }).Build();
    }

    public async Task PublishAsync(OtpEmailMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
    }

    public void Dispose() => _producer.Dispose();
}
