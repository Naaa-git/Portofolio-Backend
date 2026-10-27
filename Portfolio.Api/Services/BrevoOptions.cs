namespace Portfolio.Api.Services;

public class BrevoOptions
{
    public const string SectionName = "Brevo";

    public string ApiKey { get; set; } = default!;
    public string SenderEmail { get; set; } = default!;
    public string SenderName { get; set; } = "Portfolio Admin";

    // Alamat yang nerima notifikasi contact form (pemilik situs), beda dari
    // SenderEmail (identitas pengirim di Brevo). Fallback ke SenderEmail
    // kalau kosong, biar nggak wajib di-setup ulang di tiap environment.
    public string? OwnerNotificationEmail { get; set; }
}
