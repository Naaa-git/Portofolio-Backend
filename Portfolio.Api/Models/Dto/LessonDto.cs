namespace Portfolio.Api.Models.Dto;

public record LessonDto(
    int Id, int CategoryId, string CategoryName, string Title, string Description,
    string? ProjectSlug, string? ProjectTitle, string? ExperienceCompany);

public record LessonAdminDto(
    int Id, int CategoryId, Dictionary<string, string> Title, Dictionary<string, string> Description,
    int? ProjectId, int? ExperienceId);

public record LessonUpsertDto(
    int CategoryId, Dictionary<string, string> Title, Dictionary<string, string> Description,
    int? ProjectId, int? ExperienceId);
