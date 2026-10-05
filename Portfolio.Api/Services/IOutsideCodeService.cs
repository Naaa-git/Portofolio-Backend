using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IOutsideCodeService
{
    Task<OutsideCodeIntroDto?> GetIntroAsync(string lang);
    Task<OutsideCodeIntroAdminDto?> GetIntroAdminAsync();
    Task<OutsideCodeIntroAdminDto> UpdateIntroAsync(OutsideCodeIntroUpsertDto dto);

    Task<List<AwayFromKeyboardItemDto>> GetAwayFromKeyboardAsync(string lang);
    Task<List<AwayFromKeyboardItemAdminDto>> GetAwayFromKeyboardAdminAsync();
    Task<AwayFromKeyboardItemAdminDto> AddAwayFromKeyboardAsync(AwayFromKeyboardItemUpsertDto dto);
    Task<bool> UpdateAwayFromKeyboardAsync(int id, AwayFromKeyboardItemUpsertDto dto);
    Task<bool> DeleteAwayFromKeyboardAsync(int id);

    Task<List<MovieTakeDto>> GetMoviesAsync(string lang);
    Task<List<MovieTakeAdminDto>> GetMoviesAdminAsync();
    Task<MovieTakeAdminDto> AddMovieAsync(MovieTakeUpsertDto dto);
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

    Task<List<OutsideCodeBookDto>> GetBooksAsync(string lang);
    Task<List<OutsideCodeBookAdminDto>> GetBooksAdminAsync();
    Task<OutsideCodeBookAdminDto> AddBookAsync(OutsideCodeBookUpsertDto dto);
    Task<bool> UpdateBookAsync(int id, OutsideCodeBookUpsertDto dto);
    Task<bool> DeleteBookAsync(int id);

    Task<List<LifeInspirationDto>> GetLifeInspirationsAsync(string lang);
    Task<List<LifeInspirationAdminDto>> GetLifeInspirationsAdminAsync();
    Task<LifeInspirationAdminDto> AddLifeInspirationAsync(LifeInspirationUpsertDto dto);
    Task<bool> UpdateLifeInspirationAsync(int id, LifeInspirationUpsertDto dto);
    Task<bool> DeleteLifeInspirationAsync(int id);
}
