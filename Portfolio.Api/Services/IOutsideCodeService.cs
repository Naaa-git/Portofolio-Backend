using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IOutsideCodeService
{
    Task<OutsideCodeIntroDto?> GetIntroAsync();
    Task<OutsideCodeIntroDto> UpdateIntroAsync(OutsideCodeIntroDto dto);

    Task<List<AwayFromKeyboardItemDto>> GetAwayFromKeyboardAsync();
    Task<AwayFromKeyboardItemDto> AddAwayFromKeyboardAsync(AwayFromKeyboardItemUpsertDto dto);
    Task<bool> UpdateAwayFromKeyboardAsync(int id, AwayFromKeyboardItemUpsertDto dto);
    Task<bool> DeleteAwayFromKeyboardAsync(int id);

    Task<List<MovieTakeDto>> GetMoviesAsync();
    Task<MovieTakeDto> AddMovieAsync(MovieTakeUpsertDto dto);
    Task<bool> UpdateMovieAsync(int id, MovieTakeUpsertDto dto);
    Task<bool> DeleteMovieAsync(int id);

    Task<List<MusicArtistDto>> GetMusicArtistsAsync();
    Task<MusicArtistDto> AddMusicArtistAsync(MusicArtistUpsertDto dto);
    Task<bool> UpdateMusicArtistAsync(int id, MusicArtistUpsertDto dto);
    Task<bool> DeleteMusicArtistAsync(int id);

    Task<List<PodcastChannelDto>> GetPodcastsAsync();
    Task<PodcastChannelDto> AddPodcastAsync(PodcastChannelUpsertDto dto);
    Task<bool> UpdatePodcastAsync(int id, PodcastChannelUpsertDto dto);
    Task<bool> DeletePodcastAsync(int id);

    Task<List<OutsideCodeBookDto>> GetBooksAsync();
    Task<OutsideCodeBookDto> AddBookAsync(OutsideCodeBookUpsertDto dto);
    Task<bool> UpdateBookAsync(int id, OutsideCodeBookUpsertDto dto);
    Task<bool> DeleteBookAsync(int id);

    Task<List<LifeInspirationDto>> GetLifeInspirationsAsync();
    Task<LifeInspirationDto> AddLifeInspirationAsync(LifeInspirationUpsertDto dto);
    Task<bool> UpdateLifeInspirationAsync(int id, LifeInspirationUpsertDto dto);
    Task<bool> DeleteLifeInspirationAsync(int id);
}
