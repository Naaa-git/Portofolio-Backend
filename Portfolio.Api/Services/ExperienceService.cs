using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class ExperienceService(IExperienceRepository repo) : IExperienceService
{
    public async Task<List<ExperienceDto>> GetAllAsync() =>
        (await repo.GetAllAsync()).Select(ToDto).ToList();

    public async Task<ExperienceDto> CreateAsync(ExperienceUpsertDto dto)
    {
        var entity = new Experience
        {
            Company = dto.Company, Role = dto.Role, Type = dto.Type, Period = dto.Period,
            Location = dto.Location, Current = dto.Current, Description = dto.Description, Skills = dto.Skills,
        };
        var created = await repo.AddAsync(entity);
        return ToDto(created);
    }

    public Task<bool> UpdateAsync(int id, ExperienceUpsertDto dto) => repo.UpdateAsync(new Experience
    {
        Id = id, Company = dto.Company, Role = dto.Role, Type = dto.Type, Period = dto.Period,
        Location = dto.Location, Current = dto.Current, Description = dto.Description, Skills = dto.Skills,
    });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static ExperienceDto ToDto(Experience e) => new(
        e.Id, e.Company, e.Role, e.Type, e.Period, e.Location, e.Current, e.Description, e.Skills);
}
