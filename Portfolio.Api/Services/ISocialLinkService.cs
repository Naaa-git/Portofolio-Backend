using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface ISocialLinkService
{
    Task<List<SocialLinkDto>> GetAllAsync();
    Task<List<SocialLinkAdminDto>> GetAllAdminAsync();
    Task<SocialLinkAdminDto> CreateAsync(SocialLinkUpsertDto dto);
    Task<bool> UpdateAsync(int id, SocialLinkUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
