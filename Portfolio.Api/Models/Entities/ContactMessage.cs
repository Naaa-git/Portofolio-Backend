namespace Portfolio.Api.Models.Entities;

public enum ContactMessageStatus { Pending, Sent, Failed }

public class ContactMessage
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Message { get; set; } = default!;
    public ContactMessageStatus Status { get; set; } = ContactMessageStatus.Pending;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? SentAtUtc { get; set; }
    public string? ErrorMessage { get; set; }
}
