namespace Portfolio.Api.Models.Dto;

public record OutsideCodeIntroDto(string Paragraph1, string Paragraph2);

public record AwayFromKeyboardItemDto(int Id, string Title, string Note, string? ImageUrl, int SortOrder);
public record AwayFromKeyboardItemUpsertDto(string Title, string Note, string? ImageUrl, int SortOrder);

public record MovieTakeDto(int Id, string Title, string Take, string? ImageUrl, int SortOrder);
public record MovieTakeUpsertDto(string Title, string Take, string? ImageUrl, int SortOrder);

public record MusicArtistDto(int Id, string Name, string Url, string? ImageUrl, int SortOrder);
public record MusicArtistUpsertDto(string Name, string Url, string? ImageUrl, int SortOrder);

public record PodcastChannelDto(int Id, string Name, string Url, string? ImageUrl, int SortOrder);
public record PodcastChannelUpsertDto(string Name, string Url, string? ImageUrl, int SortOrder);

public record OutsideCodeBookDto(int Id, string Title, string Author, string? Note, string? ImageUrl, bool IsCurrentlyReading, int SortOrder);
public record OutsideCodeBookUpsertDto(string Title, string Author, string? Note, string? ImageUrl, bool IsCurrentlyReading, int SortOrder);

public record LifeInspirationDto(int Id, string Name, string Aspect, string Note, string? ImageUrl, int SortOrder);
public record LifeInspirationUpsertDto(string Name, string Aspect, string Note, string? ImageUrl, int SortOrder);
