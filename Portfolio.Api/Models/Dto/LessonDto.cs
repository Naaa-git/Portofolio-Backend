using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Validation;

namespace Portfolio.Api.Models.Dto;

public record LessonDto(
    int Id, int CategoryId, string CategoryName, string Title, string Description,
    string? ProjectSlug, string? ProjectTitle, string? ExperienceCompany);

public record LessonAdminDto(
    int Id, int CategoryId, Dictionary<string, string> Title, Dictionary<string, string> Description,
    int? ProjectId, int? ExperienceId);

public record LessonUpsertDto(
    [Range(1, int.MaxValue)] int CategoryId,
    [TranslatableStringLength(150, MinimumLength = 1)] Dictionary<string, string> Title,
    [TranslatableStringLength(2000, MinimumLength = 1)] Dictionary<string, string> Description,
    int? ProjectId,
    int? ExperienceId);
