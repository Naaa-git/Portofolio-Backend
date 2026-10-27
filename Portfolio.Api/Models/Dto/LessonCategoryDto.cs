using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Validation;

namespace Portfolio.Api.Models.Dto;

public record LessonCategoryDto(int Id, string Name);

public record LessonCategoryAdminDto(int Id, Dictionary<string, string> Name, int SortOrder);

public record LessonCategoryUpsertDto(
    [TranslatableStringLength(100, MinimumLength = 1)] Dictionary<string, string> Name,
    [Range(0, 9999)] int SortOrder);
