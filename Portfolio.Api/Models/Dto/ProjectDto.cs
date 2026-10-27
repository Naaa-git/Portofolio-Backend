using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Validation;

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
    [Required, StringLength(150)] string Title,
    [Required, StringLength(150), RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")] string Slug,
    [TranslatableStringLength(300, MinimumLength = 1)] Dictionary<string, string> ShortDescription,
    [TranslatableStringLength(500)] List<Dictionary<string, string>> Contribution,
    List<string> TechStack,
    [Required, Url, StringLength(500)] string ImageUrl,
    [Url, StringLength(500)] string? GithubUrl,
    [Url, StringLength(500)] string? DemoUrl,
    bool Featured,
    ProjectCategory Category);
