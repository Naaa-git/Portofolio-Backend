namespace Portfolio.Api.Models.Dto;

public record LessonCategoryDto(int Id, string Name);

public record LessonCategoryAdminDto(int Id, Dictionary<string, string> Name, int SortOrder);

public record LessonCategoryUpsertDto(Dictionary<string, string> Name, int SortOrder);
