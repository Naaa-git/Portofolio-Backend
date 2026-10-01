namespace Portfolio.Api.Models.Dto;

public record SocialLinkDto(int Id, string Name, string Url, string Icon);

public record SocialLinkUpsertDto(string Name, string Url, string Icon);
