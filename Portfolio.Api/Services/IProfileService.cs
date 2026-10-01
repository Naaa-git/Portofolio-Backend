using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IProfileService
{
    Task<ProfileDto?> GetAsync();
    Task<ProfileDto> UpsertAsync(ProfileUpsertDto dto);
}
