using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class ContactMessageService(IContactMessageRepository repo) : IContactMessageService
{
    public async Task<ContactMessageAdminDto> SubmitAsync(ContactMessageSubmitDto dto)
    {
        var entity = new ContactMessage { Name = dto.Name, Email = dto.Email, Message = dto.Message };
        var created = await repo.AddAsync(entity);
        return ToAdminDto(created);
    }

    public async Task<List<ContactMessageAdminDto>> GetAllAdminAsync() =>
        (await repo.GetAllAsync()).Select(ToAdminDto).ToList();

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static ContactMessageAdminDto ToAdminDto(ContactMessage m) => new(
        m.Id, m.Name, m.Email, m.Message, m.Status, m.CreatedAtUtc, m.SentAtUtc, m.ErrorMessage);
}
