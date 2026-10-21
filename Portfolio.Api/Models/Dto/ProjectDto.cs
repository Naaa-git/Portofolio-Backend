using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Models.Dto;

public record ProjectDto(
    int Id, string Title, string Slug, string ShortDescription, List<string> Contribution,
    List<string> TechStack, string ImageUrl, string? GithubUrl, string? DemoUrl,
    bool Featured, string Category, string? ExperienceCompany);

public record ProjectAdminDto(
    int Id, string Title, string Slug, Dictionary<string, string> ShortDescription, List<Dictionary<string, string>> Contribution,
    List<string> TechStack, string ImageUrl, string? GithubUrl, string? DemoUrl,
    bool Featured, ProjectCategory Category);

public record ProjectUpsertDto(
    string Title, string Slug, Dictionary<string, string> ShortDescription, List<Dictionary<string, string>> Contribution,
    List<string> TechStack, string ImageUrl, string? GithubUrl, string? DemoUrl,
    bool Featured, ProjectCategory Category);
