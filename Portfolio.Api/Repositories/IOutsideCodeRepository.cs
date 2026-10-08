using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public interface IOutsideCodeRepository
{
    Task<OutsideCodeIntro?> GetIntroAsync();
    Task<OutsideCodeIntro> UpsertIntroAsync(OutsideCodeIntro intro);

    Task<List<AwayFromKeyboardItem>> GetAwayFromKeyboardAsync();
    Task<AwayFromKeyboardItem> AddAwayFromKeyboardAsync(AwayFromKeyboardItem item);
    Task<bool> UpdateAwayFromKeyboardAsync(AwayFromKeyboardItem item);
    Task<bool> DeleteAwayFromKeyboardAsync(int id);

    Task<List<MovieTake>> GetMoviesAsync();
    Task<MovieTake> AddMovieAsync(MovieTake item);
    Task<bool> UpdateMovieAsync(MovieTake item);
    Task<bool> DeleteMovieAsync(int id);

    Task<List<MusicArtist>> GetMusicArtistsAsync();
    Task<MusicArtist> AddMusicArtistAsync(MusicArtist item);
    Task<bool> UpdateMusicArtistAsync(MusicArtist item);
    Task<bool> DeleteMusicArtistAsync(int id);

    Task<List<PodcastChannel>> GetPodcastsAsync();
    Task<PodcastChannel> AddPodcastAsync(PodcastChannel item);
    Task<bool> UpdatePodcastAsync(PodcastChannel item);
    Task<bool> DeletePodcastAsync(int id);

    Task<List<OutsideCodeBook>> GetBooksAsync();
    Task<OutsideCodeBook> AddBookAsync(OutsideCodeBook item);
    Task<bool> UpdateBookAsync(OutsideCodeBook item);
    Task<bool> DeleteBookAsync(int id);

    Task<List<LifeInspiration>> GetLifeInspirationsAsync();
    Task<LifeInspiration> AddLifeInspirationAsync(LifeInspiration item);
    Task<bool> UpdateLifeInspirationAsync(LifeInspiration item);
    Task<bool> DeleteLifeInspirationAsync(int id);
}
