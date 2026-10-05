using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class ProjectService(IProjectRepository repo) : IProjectService
{
    public async Task<List<ProjectDto>> GetAllAsync(string lang) =>
        (await repo.GetAllAsync()).Select(p => ToDto(p, lang)).ToList();

    public async Task<ProjectDto?> GetBySlugAsync(string slug, string lang)
    {
        var project = await repo.GetBySlugAsync(slug);
        return project is null ? null : ToDto(project, lang);
    }

    public async Task<List<ProjectAdminDto>> GetAllAdminAsync() =>
        (await repo.GetAllAsync()).Select(ToAdminDto).ToList();

    public async Task<ProjectAdminDto> CreateAsync(ProjectUpsertDto dto)
    {
        var entity = new Project
        {
            Title = dto.Title, Slug = dto.Slug, ShortDescription = dto.ShortDescription,
            LongDescription = dto.LongDescription, TechStack = dto.TechStack, ImageUrl = dto.ImageUrl,
            GithubUrl = dto.GithubUrl, DemoUrl = dto.DemoUrl, Featured = dto.Featured, Category = dto.Category,
        };
        var created = await repo.AddAsync(entity);
        return ToAdminDto(created);
    }

    public Task<bool> UpdateAsync(int id, ProjectUpsertDto dto) => repo.UpdateAsync(new Project
    {
        Id = id, Title = dto.Title, Slug = dto.Slug, ShortDescription = dto.ShortDescription,
        LongDescription = dto.LongDescription, TechStack = dto.TechStack, ImageUrl = dto.ImageUrl,
        GithubUrl = dto.GithubUrl, DemoUrl = dto.DemoUrl, Featured = dto.Featured, Category = dto.Category,
    });

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static ProjectDto ToDto(Project p, string lang) => new(
        p.Id, p.Title, p.Slug, p.ShortDescription.Resolve(lang), p.LongDescription.Resolve(lang), p.TechStack,
        p.ImageUrl, p.GithubUrl, p.DemoUrl, p.Featured, p.Category.Resolve(lang));

    private static ProjectAdminDto ToAdminDto(Project p) => new(
        p.Id, p.Title, p.Slug, p.ShortDescription, p.LongDescription, p.TechStack,
        p.ImageUrl, p.GithubUrl, p.DemoUrl, p.Featured, p.Category);
}
