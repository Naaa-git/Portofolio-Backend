namespace Portfolio.Api.Models.Dto;

// Resolved for a single language — what the public site consumes.
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

// Raw per-language dictionaries — what the admin form reads/writes so both
// languages can be edited side by side.
public record ProfileAdminDto(
    string Name,
    string ShortName,
    Dictionary<string, string> Role,
    List<Dictionary<string, string>> RoleAlternatives,
    Dictionary<string, string> Tagline,
    Dictionary<string, string> Bio,
    Dictionary<string, string> BioExtended,
    bool AvailableForWork,
    string Email,
    string Location,
    string AvatarUrl,
    string CvUrl);

public record ProfileUpsertDto(
    string Name,
    string ShortName,
    Dictionary<string, string> Role,
    List<Dictionary<string, string>> RoleAlternatives,
    Dictionary<string, string> Tagline,
    Dictionary<string, string> Bio,
    Dictionary<string, string> BioExtended,
    bool AvailableForWork,
    string Email,
    string Location,
    string AvatarUrl,
    string CvUrl);
