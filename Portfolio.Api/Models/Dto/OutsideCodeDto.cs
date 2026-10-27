using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Validation;

namespace Portfolio.Api.Models.Dto;

public record OutsideCodeIntroDto(string Paragraph1, string Paragraph2);
public record OutsideCodeIntroAdminDto(Dictionary<string, string> Paragraph1, Dictionary<string, string> Paragraph2);
public record OutsideCodeIntroUpsertDto(
    [TranslatableStringLength(1000, MinimumLength = 1)] Dictionary<string, string> Paragraph1,
    [TranslatableStringLength(1000, MinimumLength = 1)] Dictionary<string, string> Paragraph2);

public record AwayFromKeyboardItemDto(int Id, string Title, string Note, string? ImageUrl, int SortOrder);
public record AwayFromKeyboardItemAdminDto(int Id, Dictionary<string, string> Title, Dictionary<string, string> Note, string? ImageUrl, int SortOrder);
public record AwayFromKeyboardItemUpsertDto(
    [TranslatableStringLength(100, MinimumLength = 1)] Dictionary<string, string> Title,
    [TranslatableStringLength(500, MinimumLength = 1)] Dictionary<string, string> Note,
    [Url, StringLength(500)] string? ImageUrl,
    [Range(0, 9999)] int SortOrder);

public record MovieTakeDto(int Id, string Title, string Take, string? ImageUrl, int SortOrder);
public record MovieTakeAdminDto(int Id, string Title, Dictionary<string, string> Take, string? ImageUrl, int SortOrder);
public record MovieTakeUpsertDto(
    [Required, StringLength(150)] string Title,
    [TranslatableStringLength(1000, MinimumLength = 1)] Dictionary<string, string> Take,
    [Url, StringLength(500)] string? ImageUrl,
    [Range(0, 9999)] int SortOrder);

public record MusicArtistDto(int Id, string Name, string Url, string? ImageUrl, int SortOrder);
public record MusicArtistUpsertDto(
    [Required, StringLength(100)] string Name,
    [Required, Url, StringLength(500)] string Url,
    [Url, StringLength(500)] string? ImageUrl,
    [Range(0, 9999)] int SortOrder);

public record PodcastChannelDto(int Id, string Name, string Url, string? ImageUrl, int SortOrder);
public record PodcastChannelUpsertDto(
    [Required, StringLength(100)] string Name,
    [Required, Url, StringLength(500)] string Url,
    [Url, StringLength(500)] string? ImageUrl,
    [Range(0, 9999)] int SortOrder);

public record OutsideCodeBookDto(int Id, string Title, string Author, string? Note, string? ImageUrl, bool IsCurrentlyReading, int SortOrder);
public record OutsideCodeBookAdminDto(int Id, string Title, string Author, Dictionary<string, string> Note, string? ImageUrl, bool IsCurrentlyReading, int SortOrder);
public record OutsideCodeBookUpsertDto(
    [Required, StringLength(150)] string Title,
    [Required, StringLength(100)] string Author,
    [TranslatableStringLength(500)] Dictionary<string, string> Note,
    [Url, StringLength(500)] string? ImageUrl,
    bool IsCurrentlyReading,
    [Range(0, 9999)] int SortOrder);

public record LifeInspirationDto(int Id, string Name, string Aspect, string Note, string? ImageUrl, int SortOrder);
public record LifeInspirationAdminDto(int Id, string Name, Dictionary<string, string> Aspect, Dictionary<string, string> Note, string? ImageUrl, int SortOrder);
public record LifeInspirationUpsertDto(
    [Required, StringLength(100)] string Name,
    [TranslatableStringLength(150, MinimumLength = 1)] Dictionary<string, string> Aspect,
    [TranslatableStringLength(1000, MinimumLength = 1)] Dictionary<string, string> Note,
    [Url, StringLength(500)] string? ImageUrl,
    [Range(0, 9999)] int SortOrder);
