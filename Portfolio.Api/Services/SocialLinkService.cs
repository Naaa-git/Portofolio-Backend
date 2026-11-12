using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class SocialLinkService(ISocialLinkRepository repo) : ISocialLinkService
{
    public async Task<List<SocialLinkDto>> GetAllAsync() =>
        (await repo.GetAllAsync()).Where(s => s.IsPublished).Select(ToDto).ToList();

    public async Task<List<SocialLinkAdminDto>> GetAllAdminAsync() =>
        (await repo.GetAllAsync()).Select(ToAdminDto).ToList();

    public async Task<SocialLinkAdminDto> CreateAsync(SocialLinkUpsertDto dto)
    {
        var entity = new SocialLink { Name = dto.Name, Url = dto.Url, Icon = dto.Icon, IsPublished = dto.IsPublished };
        var created = await repo.AddAsync(entity);
        return ToAdminDto(created);
    }

    public Task<bool> UpdateAsync(int id, SocialLinkUpsertDto dto) =>
        repo.UpdateAsync(new SocialLink { Id = id, Name = dto.Name, Url = dto.Url, Icon = dto.Icon, IsPublished = dto.IsPublished });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static SocialLinkDto ToDto(SocialLink s) => new(s.Id, s.Name, s.Url, s.Icon);

    private static SocialLinkAdminDto ToAdminDto(SocialLink s) => new(s.Id, s.Name, s.Url, s.Icon, s.IsPublished);
}
