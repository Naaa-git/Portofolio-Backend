using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IContactMessageService
{
    Task<ContactMessageAdminDto> SubmitAsync(ContactMessageSubmitDto dto);
    Task<List<ContactMessageAdminDto>> GetAllAdminAsync();
    Task<bool> DeleteAsync(int id);
}
