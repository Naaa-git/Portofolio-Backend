using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class LessonService(ILessonRepository repo) : ILessonService
{
    public async Task<List<LessonDto>> GetAllAsync(string lang) =>
        (await repo.GetAllAsync()).Select(l => ToDto(l, lang)).ToList();

    public async Task<List<LessonAdminDto>> GetAllAdminAsync() =>
        (await repo.GetAllAsync()).Select(ToAdminDto).ToList();

    public async Task<LessonAdminDto> CreateAsync(LessonUpsertDto dto)
    {
        var entity = new Lesson
        {
            CategoryId = dto.CategoryId, Title = dto.Title, Description = dto.Description,
            ProjectId = dto.ProjectId, ExperienceId = dto.ExperienceId,
        };
        var created = await repo.AddAsync(entity);
        return ToAdminDto(created);
    }

    public Task<bool> UpdateAsync(int id, LessonUpsertDto dto) =>
        repo.UpdateAsync(new Lesson
        {
            Id = id, CategoryId = dto.CategoryId, Title = dto.Title, Description = dto.Description,
            ProjectId = dto.ProjectId, ExperienceId = dto.ExperienceId,
        });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static LessonDto ToDto(Lesson l, string lang) => new(
        l.Id, l.CategoryId, l.Category.Name.Resolve(lang), l.Title.Resolve(lang), l.Description.Resolve(lang),
        l.Project?.Slug, l.Project?.Title, l.Experience?.Company);

    private static LessonAdminDto ToAdminDto(Lesson l) => new(
        l.Id, l.CategoryId, l.Title, l.Description, l.ProjectId, l.ExperienceId);
}
