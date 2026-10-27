using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Validation;

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
    [Required, StringLength(100)] string Name,
    [Required, StringLength(50)] string ShortName,
    [TranslatableStringLength(100, MinimumLength = 1)] Dictionary<string, string> Role,
    [TranslatableStringLength(100)] List<Dictionary<string, string>> RoleAlternatives,
    [TranslatableStringLength(200, MinimumLength = 1)] Dictionary<string, string> Tagline,
    [TranslatableStringLength(1000, MinimumLength = 1)] Dictionary<string, string> Bio,
    [TranslatableStringLength(3000, MinimumLength = 1)] Dictionary<string, string> BioExtended,
    bool AvailableForWork,
    [Required, EmailAddress, StringLength(254)] string Email,
    [Required, StringLength(150)] string Location,
    [Required, Url, StringLength(500)] string AvatarUrl,
    [Required, Url, StringLength(500)] string CvUrl);
