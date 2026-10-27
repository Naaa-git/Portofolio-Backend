using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Validation;

namespace Portfolio.Api.Models.Dto;

public record ProjectLinkDto(string Slug, string Title);

public record ExperienceDto(
    int Id, string Company, string Role, string Type, DateOnly StartDate, DateOnly? EndDate,
    string Location, bool Current, string Description, List<string> Skills, List<ProjectLinkDto> Projects);

public record ExperienceAdminDto(
    int Id, string Company, Dictionary<string, string> Role, string Type, DateOnly StartDate, DateOnly? EndDate,
    string Location, bool Current, Dictionary<string, string> Description, List<string> Skills, List<int> ProjectIds);

public record ExperienceUpsertDto(
    [Required, StringLength(150)] string Company,
    [TranslatableStringLength(150, MinimumLength = 1)] Dictionary<string, string> Role,
    [Required, StringLength(50)] string Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    [Required, StringLength(150)] string Location,
    bool Current,
    [TranslatableStringLength(2000, MinimumLength = 1)] Dictionary<string, string> Description,
    List<string> Skills) : IValidatableObject
{
    // Null EndDate means "still ongoing", paired with Current — see Experience.cs.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Current && EndDate is not null)
            yield return new ValidationResult(
                "EndDate harus kosong kalau Current = true (posisi masih berjalan).", [nameof(EndDate)]);

        if (!Current && EndDate is null)
            yield return new ValidationResult(
                "EndDate wajib diisi kalau Current = false.", [nameof(EndDate)]);
    }
}
