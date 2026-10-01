using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class SocialLinkService(ISocialLinkRepository repo) : ISocialLinkService
{
    public async Task<List<SocialLinkDto>> GetAllAsync() =>
        (await repo.GetAllAsync()).Select(ToDto).ToList();

    public async Task<SocialLinkDto> CreateAsync(SocialLinkUpsertDto dto)
    {
        var entity = new SocialLink { Name = dto.Name, Url = dto.Url, Icon = dto.Icon };
        var created = await repo.AddAsync(entity);
        return ToDto(created);
    }

    public Task<bool> UpdateAsync(int id, SocialLinkUpsertDto dto) =>
        repo.UpdateAsync(new SocialLink { Id = id, Name = dto.Name, Url = dto.Url, Icon = dto.Icon });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static SocialLinkDto ToDto(SocialLink s) => new(s.Id, s.Name, s.Url, s.Icon);
}
