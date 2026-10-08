using Portfolio.Api.Data;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Entities;
using Portfolio.Api.Repositories;

namespace Portfolio.Api.Services;

public class OutsideCodeService(IOutsideCodeRepository repo) : IOutsideCodeService
{
    // --- Intro ---

    public async Task<OutsideCodeIntroDto?> GetIntroAsync(string lang)
    {
        var intro = await repo.GetIntroAsync();
        return intro is null ? null : new OutsideCodeIntroDto(intro.Paragraph1.Resolve(lang), intro.Paragraph2.Resolve(lang));
    }

    public async Task<OutsideCodeIntroAdminDto?> GetIntroAdminAsync()
    {
        var intro = await repo.GetIntroAsync();
        return intro is null ? null : new OutsideCodeIntroAdminDto(intro.Paragraph1, intro.Paragraph2);
    }

    public async Task<OutsideCodeIntroAdminDto> UpdateIntroAsync(OutsideCodeIntroUpsertDto dto)
    {
        var saved = await repo.UpsertIntroAsync(new OutsideCodeIntro { Paragraph1 = dto.Paragraph1, Paragraph2 = dto.Paragraph2 });
        return new OutsideCodeIntroAdminDto(saved.Paragraph1, saved.Paragraph2);
    }

    // --- Away From the Keyboard ---

    public async Task<List<AwayFromKeyboardItemDto>> GetAwayFromKeyboardAsync(string lang) =>
        (await repo.GetAwayFromKeyboardAsync())
            .Select(x => new AwayFromKeyboardItemDto(x.Id, x.Title.Resolve(lang), x.Note.Resolve(lang), x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<List<AwayFromKeyboardItemAdminDto>> GetAwayFromKeyboardAdminAsync() =>
        (await repo.GetAwayFromKeyboardAsync())
            .Select(x => new AwayFromKeyboardItemAdminDto(x.Id, x.Title, x.Note, x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<AwayFromKeyboardItemAdminDto> AddAwayFromKeyboardAsync(AwayFromKeyboardItemUpsertDto dto)
    {
        var created = await repo.AddAwayFromKeyboardAsync(new AwayFromKeyboardItem
        {
            Title = dto.Title, Note = dto.Note, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });
        return new AwayFromKeyboardItemAdminDto(created.Id, created.Title, created.Note, created.ImageUrl, created.SortOrder);
    }

    public Task<bool> UpdateAwayFromKeyboardAsync(int id, AwayFromKeyboardItemUpsertDto dto) =>
        repo.UpdateAwayFromKeyboardAsync(new AwayFromKeyboardItem
        {
            Id = id, Title = dto.Title, Note = dto.Note, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });

    public Task<bool> DeleteAwayFromKeyboardAsync(int id) => repo.DeleteAwayFromKeyboardAsync(id);

    // --- Movies & Shows ---

    public async Task<List<MovieTakeDto>> GetMoviesAsync(string lang) =>
        (await repo.GetMoviesAsync())
            .Select(x => new MovieTakeDto(x.Id, x.Title, x.Take.Resolve(lang), x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<List<MovieTakeAdminDto>> GetMoviesAdminAsync() =>
        (await repo.GetMoviesAsync())
            .Select(x => new MovieTakeAdminDto(x.Id, x.Title, x.Take, x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<MovieTakeAdminDto> AddMovieAsync(MovieTakeUpsertDto dto)
    {
        var created = await repo.AddMovieAsync(new MovieTake
        {
            Title = dto.Title, Take = dto.Take, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });
        return new MovieTakeAdminDto(created.Id, created.Title, created.Take, created.ImageUrl, created.SortOrder);
    }

    public Task<bool> UpdateMovieAsync(int id, MovieTakeUpsertDto dto) =>
        repo.UpdateMovieAsync(new MovieTake
        {
            Id = id, Title = dto.Title, Take = dto.Take, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });

    public Task<bool> DeleteMovieAsync(int id) => repo.DeleteMovieAsync(id);

    // --- Music Artists (no translatable fields) ---

    public async Task<List<MusicArtistDto>> GetMusicArtistsAsync() =>
        (await repo.GetMusicArtistsAsync())
            .Select(x => new MusicArtistDto(x.Id, x.Name, x.Url, x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<MusicArtistDto> AddMusicArtistAsync(MusicArtistUpsertDto dto)
    {
        var created = await repo.AddMusicArtistAsync(new MusicArtist
        {
            Name = dto.Name, Url = dto.Url, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });
        return new MusicArtistDto(created.Id, created.Name, created.Url, created.ImageUrl, created.SortOrder);
    }

    public Task<bool> UpdateMusicArtistAsync(int id, MusicArtistUpsertDto dto) =>
        repo.UpdateMusicArtistAsync(new MusicArtist
        {
            Id = id, Name = dto.Name, Url = dto.Url, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });

    public Task<bool> DeleteMusicArtistAsync(int id) => repo.DeleteMusicArtistAsync(id);

    // --- Podcasts (no translatable fields) ---

    public async Task<List<PodcastChannelDto>> GetPodcastsAsync() =>
        (await repo.GetPodcastsAsync())
            .Select(x => new PodcastChannelDto(x.Id, x.Name, x.Url, x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<PodcastChannelDto> AddPodcastAsync(PodcastChannelUpsertDto dto)
    {
        var created = await repo.AddPodcastAsync(new PodcastChannel
        {
            Name = dto.Name, Url = dto.Url, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });
        return new PodcastChannelDto(created.Id, created.Name, created.Url, created.ImageUrl, created.SortOrder);
    }

    public Task<bool> UpdatePodcastAsync(int id, PodcastChannelUpsertDto dto) =>
        repo.UpdatePodcastAsync(new PodcastChannel
        {
            Id = id, Name = dto.Name, Url = dto.Url, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });

    public Task<bool> DeletePodcastAsync(int id) => repo.DeletePodcastAsync(id);

    // --- Books ---

    public async Task<List<OutsideCodeBookDto>> GetBooksAsync(string lang) =>
        (await repo.GetBooksAsync())
            .Select(x => new OutsideCodeBookDto(x.Id, x.Title, x.Author, x.Note.Resolve(lang), x.ImageUrl, x.IsCurrentlyReading, x.SortOrder))
            .ToList();

    public async Task<List<OutsideCodeBookAdminDto>> GetBooksAdminAsync() =>
        (await repo.GetBooksAsync())
            .Select(x => new OutsideCodeBookAdminDto(x.Id, x.Title, x.Author, x.Note, x.ImageUrl, x.IsCurrentlyReading, x.SortOrder))
            .ToList();

    public async Task<OutsideCodeBookAdminDto> AddBookAsync(OutsideCodeBookUpsertDto dto)
    {
        var created = await repo.AddBookAsync(new OutsideCodeBook
        {
            Title = dto.Title, Author = dto.Author, Note = dto.Note, ImageUrl = dto.ImageUrl,
            IsCurrentlyReading = dto.IsCurrentlyReading, SortOrder = dto.SortOrder,
        });
        return new OutsideCodeBookAdminDto(created.Id, created.Title, created.Author, created.Note, created.ImageUrl, created.IsCurrentlyReading, created.SortOrder);
    }

    public Task<bool> UpdateBookAsync(int id, OutsideCodeBookUpsertDto dto) =>
        repo.UpdateBookAsync(new OutsideCodeBook
        {
            Id = id, Title = dto.Title, Author = dto.Author, Note = dto.Note, ImageUrl = dto.ImageUrl,
            IsCurrentlyReading = dto.IsCurrentlyReading, SortOrder = dto.SortOrder,
        });

    public Task<bool> DeleteBookAsync(int id) => repo.DeleteBookAsync(id);

    // --- Life Inspirations ---

    public async Task<List<LifeInspirationDto>> GetLifeInspirationsAsync(string lang) =>
        (await repo.GetLifeInspirationsAsync())
            .Select(x => new LifeInspirationDto(x.Id, x.Name, x.Aspect.Resolve(lang), x.Note.Resolve(lang), x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<List<LifeInspirationAdminDto>> GetLifeInspirationsAdminAsync() =>
        (await repo.GetLifeInspirationsAsync())
            .Select(x => new LifeInspirationAdminDto(x.Id, x.Name, x.Aspect, x.Note, x.ImageUrl, x.SortOrder))
            .ToList();

    public async Task<LifeInspirationAdminDto> AddLifeInspirationAsync(LifeInspirationUpsertDto dto)
    {
        var created = await repo.AddLifeInspirationAsync(new LifeInspiration
        {
            Name = dto.Name, Aspect = dto.Aspect, Note = dto.Note, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });
        return new LifeInspirationAdminDto(created.Id, created.Name, created.Aspect, created.Note, created.ImageUrl, created.SortOrder);
    }

    public Task<bool> UpdateLifeInspirationAsync(int id, LifeInspirationUpsertDto dto) =>
        repo.UpdateLifeInspirationAsync(new LifeInspiration
        {
            Id = id, Name = dto.Name, Aspect = dto.Aspect, Note = dto.Note, ImageUrl = dto.ImageUrl, SortOrder = dto.SortOrder,
        });

    public Task<bool> DeleteLifeInspirationAsync(int id) => repo.DeleteLifeInspirationAsync(id);
}
