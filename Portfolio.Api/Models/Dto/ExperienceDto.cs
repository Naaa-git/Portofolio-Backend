namespace Portfolio.Api.Models.Dto;

public record ProjectLinkDto(string Slug, string Title);

public record ExperienceDto(
    int Id, string Company, string Role, string Type, DateOnly StartDate, DateOnly? EndDate,
    string Location, bool Current, string Description, List<string> Skills, List<ProjectLinkDto> Projects);

public record ExperienceAdminDto(
    int Id, string Company, Dictionary<string, string> Role, string Type, DateOnly StartDate, DateOnly? EndDate,
    string Location, bool Current, Dictionary<string, string> Description, List<string> Skills, List<int> ProjectIds);

public record ExperienceUpsertDto(
    string Company, Dictionary<string, string> Role, string Type, DateOnly StartDate, DateOnly? EndDate,
    string Location, bool Current, Dictionary<string, string> Description, List<string> Skills);
