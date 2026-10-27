using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Models.Events;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class ContactEmailConsumer(
    IOptions<KafkaOptions> options,
    IServiceScopeFactory scopeFactory,
    IContactEmailProducer contactEmailProducer,
    ILogger<ContactEmailConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = "contact-email-consumer",
            EnableAutoCommit = false,
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe(options.Value.ContactEmailTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);
            var payload = JsonSerializer.Deserialize<ContactEmailMessage>(result.Message.Value)!;

            // Scope baru per pesan — IEmailSender itu HttpClient-backed, dan
            // repository ini butuh AppDbContext scoped, jangan diresolve
            // langsung dari constructor Singleton ini (captive dependency).
            using var scope = scopeFactory.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            var repo = scope.ServiceProvider.GetRequiredService<IContactMessageRepository>();

            var message = await repo.GetByIdAsync(payload.ContactMessageId);
            if (message is null)
            {
                // Row kehapus admin sebelum consumer sempat proses — nggak ada
                // yang bisa dikerjain, commit aja dan lanjut.
                logger.LogWarning("ContactMessage {Id} nggak ketemu, dilewatin", payload.ContactMessageId);
                consumer.Commit(result);
                continue;
            }

            try
            {
                await emailSender.SendContactMessageAsync(message.Name, message.Email, message.Message);
                await repo.UpdateStatusAsync(message.Id, ContactMessageStatus.Sent, null, DateTime.UtcNow);
                logger.LogInformation("Contact email terkirim untuk ContactMessage {Id}", message.Id);
                AppMetrics.ContactEmailsSent.Inc();
            }
            catch (Exception ex)
            {
                // Beda dari OTP: status Failed ditulis balik ke DB DULU — ini
                // yang jadi recovery path admin, bukan DLQ. DLQ di bawah cuma
                // telemetry tambahan, nggak boleh skip status write-back
                // meskipun publish DLQ-nya nanti gagal.
                logger.LogError(ex, "Gagal kirim contact email untuk ContactMessage {Id}", message.Id);
                AppMetrics.ContactEmailFailures.Inc();

                try
                {
                    await repo.UpdateStatusAsync(message.Id, ContactMessageStatus.Failed, ex.Message, null);
                }
                catch (Exception dbEx)
                {
                    // DB lagi down total — row tetap Pending selamanya, batasan
                    // yang diterima, jangan crash consumer-nya.
                    logger.LogError(dbEx, "Gagal update status Failed untuk ContactMessage {Id}", message.Id);
                }

                try
                {
                    await contactEmailProducer.PublishToDlqAsync(
                        new ContactEmailDlqMessage(message.Id, ex.Message, DateTime.UtcNow));
                }
                catch (Exception dlqEx)
                {
                    logger.LogError(dlqEx, "Gagal publish ke DLQ untuk ContactMessage {Id}", message.Id);
                }
            }

            consumer.Commit(result);

            var watermark = consumer.QueryWatermarkOffsets(result.TopicPartition, TimeSpan.FromSeconds(5));
            var lag = watermark.High.Value - consumer.Position(result.TopicPartition).Value;
            AppMetrics.ContactConsumerLag.Set(lag);
        }
    }
}
