namespace Portfolio.Api.Services;

public class KafkaOptions
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; set; } = "localhost:29092";
    public string OtpEmailTopic { get; set; } = "send-otp-email";
}
