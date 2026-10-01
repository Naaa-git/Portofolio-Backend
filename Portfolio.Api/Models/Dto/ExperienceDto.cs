namespace Portfolio.Api.Models.Dto;

public record ExperienceDto(
    int Id, string Company, string Role, string Type, string Period,
    string Location, bool Current, List<string> Description, List<string> Skills);

public record ExperienceUpsertDto(
    string Company, string Role, string Type, string Period,
    string Location, bool Current, List<string> Description, List<string> Skills);
