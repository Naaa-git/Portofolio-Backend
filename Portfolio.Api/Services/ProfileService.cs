using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class ProfileService(IProfileRepository repo) : IProfileService
{
    public async Task<ProfileDto?> GetAsync(string lang)
    {
        var profile = await repo.GetAsync();
        return profile is null ? null : ToDto(profile, lang);
    }

    public async Task<ProfileAdminDto?> GetAdminAsync()
    {
        var profile = await repo.GetAsync();
        return profile is null ? null : ToAdminDto(profile);
    }

    public async Task<ProfileAdminDto> UpsertAsync(ProfileUpsertDto dto)
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
        return ToAdminDto(saved);
    }

    private static ProfileDto ToDto(Profile p, string lang) => new(
        p.Name, p.ShortName, p.Role.Resolve(lang), p.RoleAlternatives.ResolveList(lang),
        p.Tagline.Resolve(lang), p.Bio.Resolve(lang), p.BioExtended.Resolve(lang),
        p.AvailableForWork, p.Email, p.Location, p.AvatarUrl, p.CvUrl);

    private static ProfileAdminDto ToAdminDto(Profile p) => new(
        p.Name, p.ShortName, p.Role, p.RoleAlternatives, p.Tagline, p.Bio, p.BioExtended,
        p.AvailableForWork, p.Email, p.Location, p.AvatarUrl, p.CvUrl);
}
