using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class LessonCategoryService(ILessonCategoryRepository repo) : ILessonCategoryService
{
    public async Task<List<LessonCategoryDto>> GetAllAsync(string lang) =>
        (await repo.GetAllAsync()).Select(c => ToDto(c, lang)).ToList();

    public async Task<List<LessonCategoryAdminDto>> GetAllAdminAsync() =>
        (await repo.GetAllAsync()).Select(ToAdminDto).ToList();

    public async Task<LessonCategoryAdminDto> CreateAsync(LessonCategoryUpsertDto dto)
    {
        var entity = new LessonCategory { Name = dto.Name, SortOrder = dto.SortOrder };
        var created = await repo.AddAsync(entity);
        return ToAdminDto(created);
    }

    public Task<bool> UpdateAsync(int id, LessonCategoryUpsertDto dto) =>
        repo.UpdateAsync(new LessonCategory { Id = id, Name = dto.Name, SortOrder = dto.SortOrder });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static LessonCategoryDto ToDto(LessonCategory c, string lang) => new(c.Id, c.Name.Resolve(lang));

    private static LessonCategoryAdminDto ToAdminDto(LessonCategory c) => new(c.Id, c.Name, c.SortOrder);
}
