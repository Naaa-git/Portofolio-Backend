using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Validation;

namespace Portfolio.Api.Models.Dto;

public record SkillDto(int Id, string Category, List<string> Items, int SortOrder);

public record SkillAdminDto(int Id, Dictionary<string, string> Category, List<string> Items, int SortOrder);

public record SkillUpsertDto(
    [TranslatableStringLength(100, MinimumLength = 1)] Dictionary<string, string> Category,
    List<string> Items,
    [Range(0, 9999)] int SortOrder);
