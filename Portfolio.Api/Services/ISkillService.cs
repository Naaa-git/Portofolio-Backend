using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface ISkillService
{
    Task<List<SkillDto>> GetAllAsync();
    Task<SkillDto> CreateAsync(SkillUpsertDto dto);
    Task<bool> UpdateAsync(int id, SkillUpsertDto dto);
    Task<bool> DeleteAsync(int id);
}
