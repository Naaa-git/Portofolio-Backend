using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class SkillService(ISkillRepository repo) : ISkillService
{
    public async Task<List<SkillDto>> GetAllAsync() =>
        (await repo.GetAllAsync()).Select(ToDto).ToList();

    public async Task<SkillDto> CreateAsync(SkillUpsertDto dto)
    {
        var entity = new Skill { Category = dto.Category, Items = dto.Items, SortOrder = dto.SortOrder };
        var created = await repo.AddAsync(entity);
        return ToDto(created);
    }

    public Task<bool> UpdateAsync(int id, SkillUpsertDto dto) =>
        repo.UpdateAsync(new Skill { Id = id, Category = dto.Category, Items = dto.Items, SortOrder = dto.SortOrder });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static SkillDto ToDto(Skill s) => new(s.Id, s.Category, s.Items, s.SortOrder);
}
