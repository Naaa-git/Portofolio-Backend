using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IProfileService
{
    Task<ProfileDto?> GetAsync(string lang);
    Task<ProfileAdminDto?> GetAdminAsync();
    Task<ProfileAdminDto> UpsertAsync(ProfileUpsertDto dto);
}
