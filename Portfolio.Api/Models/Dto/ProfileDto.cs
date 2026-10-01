namespace Portfolio.Api.Models.Dto;

public record ProfileDto(
    string Name,
    string ShortName,
    string Role,
    List<string> RoleAlternatives,
    string Tagline,
    string Bio,
    string BioExtended,
    bool AvailableForWork,
    string Email,
    string Location,
    string AvatarUrl,
    string CvUrl);

public record ProfileUpsertDto(
    string Name,
    string ShortName,
    string Role,
    List<string> RoleAlternatives,
    string Tagline,
    string Bio,
    string BioExtended,
    bool AvailableForWork,
    string Email,
    string Location,
    string AvatarUrl,
    string CvUrl);
