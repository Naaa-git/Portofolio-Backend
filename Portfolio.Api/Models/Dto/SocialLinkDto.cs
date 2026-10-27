using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Dto;

public record SocialLinkDto(int Id, string Name, string Url, string Icon);

public record SocialLinkUpsertDto(
    [Required, StringLength(50)] string Name,
    [Required, Url, StringLength(500)] string Url,
    [Required, StringLength(50)] string Icon);
