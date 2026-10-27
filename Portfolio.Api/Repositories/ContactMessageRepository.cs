using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class ContactMessageRepository(AppDbContext db) : IContactMessageRepository
{
    public async Task<ContactMessage> AddAsync(ContactMessage message)
    {
        db.ContactMessages.Add(message);
        await db.SaveChangesAsync();
        return message;
    }

    public Task<ContactMessage?> GetByIdAsync(int id) => db.ContactMessages.FindAsync(id).AsTask();

    public Task<List<ContactMessage>> GetAllAsync() =>
        db.ContactMessages.AsNoTracking().OrderByDescending(m => m.CreatedAtUtc).ToListAsync();

    public async Task UpdateStatusAsync(int id, ContactMessageStatus status, string? errorMessage, DateTime? sentAtUtc)
    {
        var message = await db.ContactMessages.FindAsync(id);
        if (message is null) return;
        message.Status = status;
        message.ErrorMessage = errorMessage;
        message.SentAtUtc = sentAtUtc;
        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var message = await db.ContactMessages.FindAsync(id);
        if (message is null) return false;
        db.ContactMessages.Remove(message);
        return await db.SaveChangesAsync() > 0;
    }
}
