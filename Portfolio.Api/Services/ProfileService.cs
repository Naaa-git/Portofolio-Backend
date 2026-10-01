using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class ProfileService(IProfileRepository repo) : IProfileService
{
    public async Task<ProfileDto?> GetAsync()
    {
        var profile = await repo.GetAsync();
        return profile is null ? null : ToDto(profile);
    }

    public async Task<ProfileDto> UpsertAsync(ProfileUpsertDto dto)
    {
        var entity = new Profile
        {
            Name = dto.Name,
            ShortName = dto.ShortName,
            Role = dto.Role,
            RoleAlternatives = dto.RoleAlternatives,
            Tagline = dto.Tagline,
            Bio = dto.Bio,
            BioExtended = dto.BioExtended,
            AvailableForWork = dto.AvailableForWork,
            Email = dto.Email,
            Location = dto.Location,
            AvatarUrl = dto.AvatarUrl,
            CvUrl = dto.CvUrl,
        };
        var saved = await repo.UpsertAsync(entity);
        return ToDto(saved);
    }

    private static ProfileDto ToDto(Profile p) => new(
        p.Name, p.ShortName, p.Role, p.RoleAlternatives, p.Tagline, p.Bio, p.BioExtended,
        p.AvailableForWork, p.Email, p.Location, p.AvatarUrl, p.CvUrl);
}
