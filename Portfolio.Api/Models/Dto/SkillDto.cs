namespace Portfolio.Api.Models.Dto;

public record SkillDto(int Id, string Category, List<string> Items, int SortOrder);

public record SkillAdminDto(int Id, Dictionary<string, string> Category, List<string> Items, int SortOrder);

public record SkillUpsertDto(Dictionary<string, string> Category, List<string> Items, int SortOrder);
