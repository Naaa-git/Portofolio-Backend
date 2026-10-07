using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Portfolio.Api.Models.Events;

namespace Portfolio.Api.Services;

public class OtpEmailConsumer(
    IOptions<KafkaOptions> options,
    IServiceScopeFactory scopeFactory,
    ILogger<OtpEmailConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = "otp-email-consumer",
            EnableAutoCommit = false, // kita kontrol kapan commit — lihat diskusi Fase 2
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe(options.Value.OtpEmailTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);
            var message = JsonSerializer.Deserialize<OtpEmailMessage>(result.Message.Value)!;

            // Scope baru per pesan — IEmailSender itu Transient, jangan diresolve
            // langsung dari constructor Singleton ini (captive dependency).
            using var scope = scopeFactory.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            try
            {
                await emailSender.SendOtpCodeAsync(message.Email, message.Code);
                logger.LogInformation("OTP email terkirim ke {Email}", message.Email);
                AppMetrics.OtpEmailsSent.Inc();
            }
            catch (Exception ex)
            {
                // Tidak di-retry: commit tetap jalan. OTP expired dalam hitungan
                // menit, jadi retry nanti (setelah redelivery) nggak ada gunanya —
                // yang penting kegagalan ini kelihatan lewat metric, bukan hilang diam-diam.
                logger.LogError(ex, "Gagal kirim OTP email ke {Email}", message.Email);
                AppMetrics.OtpEmailFailures.Inc();
            }

            consumer.Commit(result);
        }
    }
}
