using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IContactMessageRepository
{
    Task<ContactMessage> AddAsync(ContactMessage message);
    Task<ContactMessage?> GetByIdAsync(int id);
    Task<List<ContactMessage>> GetAllAsync();
    Task UpdateStatusAsync(int id, ContactMessageStatus status, string? errorMessage, DateTime? sentAtUtc);
    Task<bool> DeleteAsync(int id);
}
