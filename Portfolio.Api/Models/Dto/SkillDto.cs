namespace Portfolio.Api.Models.Dto;

public record SkillDto(int Id, string Category, List<string> Items, int SortOrder);

public record SkillUpsertDto(string Category, List<string> Items, int SortOrder);
