using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Repositories;

public class OutsideCodeRepository(AppDbContext db) : IOutsideCodeRepository
{
    // --- Intro (singleton, same upsert pattern as ProfileRepository) ---

    public Task<OutsideCodeIntro?> GetIntroAsync() =>
        db.OutsideCodeIntros.AsNoTracking().FirstOrDefaultAsync();

    public async Task<OutsideCodeIntro> UpsertIntroAsync(OutsideCodeIntro intro)
    {
        var existing = await db.OutsideCodeIntros.FirstOrDefaultAsync();
        if (existing is null)
        {
            db.OutsideCodeIntros.Add(intro);
            await db.SaveChangesAsync();
            return intro;
        }

        intro.Id = existing.Id;
        db.Entry(existing).CurrentValues.SetValues(intro);
        await db.SaveChangesAsync();
        return existing;
    }

    // --- Away From the Keyboard ---

    public Task<List<AwayFromKeyboardItem>> GetAwayFromKeyboardAsync() =>
        db.AwayFromKeyboardItems.AsNoTracking().OrderBy(x => x.SortOrder).ToListAsync();

    public async Task<AwayFromKeyboardItem> AddAwayFromKeyboardAsync(AwayFromKeyboardItem item)
    {
        db.AwayFromKeyboardItems.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateAwayFromKeyboardAsync(AwayFromKeyboardItem item)
    {
        db.AwayFromKeyboardItems.Update(item);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAwayFromKeyboardAsync(int id)
    {
        var item = await db.AwayFromKeyboardItems.FindAsync(id);
        if (item is null) return false;
        db.AwayFromKeyboardItems.Remove(item);
        return await db.SaveChangesAsync() > 0;
    }

    // --- Movies & Shows ---

    public Task<List<MovieTake>> GetMoviesAsync() =>
        db.MovieTakes.AsNoTracking().OrderBy(x => x.SortOrder).ToListAsync();

    public async Task<MovieTake> AddMovieAsync(MovieTake item)
    {
        db.MovieTakes.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateMovieAsync(MovieTake item)
    {
        db.MovieTakes.Update(item);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        var item = await db.MovieTakes.FindAsync(id);
        if (item is null) return false;
        db.MovieTakes.Remove(item);
        return await db.SaveChangesAsync() > 0;
    }

    // --- Music Artists ---

    public Task<List<MusicArtist>> GetMusicArtistsAsync() =>
        db.MusicArtists.AsNoTracking().OrderBy(x => x.SortOrder).ToListAsync();

    public async Task<MusicArtist> AddMusicArtistAsync(MusicArtist item)
    {
        db.MusicArtists.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateMusicArtistAsync(MusicArtist item)
    {
        db.MusicArtists.Update(item);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteMusicArtistAsync(int id)
    {
        var item = await db.MusicArtists.FindAsync(id);
        if (item is null) return false;
        db.MusicArtists.Remove(item);
        return await db.SaveChangesAsync() > 0;
    }

    // --- Podcasts ---

    public Task<List<PodcastChannel>> GetPodcastsAsync() =>
        db.PodcastChannels.AsNoTracking().OrderBy(x => x.SortOrder).ToListAsync();

    public async Task<PodcastChannel> AddPodcastAsync(PodcastChannel item)
    {
        db.PodcastChannels.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdatePodcastAsync(PodcastChannel item)
    {
        db.PodcastChannels.Update(item);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeletePodcastAsync(int id)
    {
        var item = await db.PodcastChannels.FindAsync(id);
        if (item is null) return false;
        db.PodcastChannels.Remove(item);
        return await db.SaveChangesAsync() > 0;
    }

    // --- Books ---

    public Task<List<OutsideCodeBook>> GetBooksAsync() =>
        db.OutsideCodeBooks.AsNoTracking().OrderBy(x => x.SortOrder).ToListAsync();

    public async Task<OutsideCodeBook> AddBookAsync(OutsideCodeBook item)
    {
        db.OutsideCodeBooks.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateBookAsync(OutsideCodeBook item)
    {
        db.OutsideCodeBooks.Update(item);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var item = await db.OutsideCodeBooks.FindAsync(id);
        if (item is null) return false;
        db.OutsideCodeBooks.Remove(item);
        return await db.SaveChangesAsync() > 0;
    }

    // --- Life Inspirations ---

    public Task<List<LifeInspiration>> GetLifeInspirationsAsync() =>
        db.LifeInspirations.AsNoTracking().OrderBy(x => x.SortOrder).ToListAsync();

    public async Task<LifeInspiration> AddLifeInspirationAsync(LifeInspiration item)
    {
        db.LifeInspirations.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateLifeInspirationAsync(LifeInspiration item)
    {
        db.LifeInspirations.Update(item);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteLifeInspirationAsync(int id)
    {
        var item = await db.LifeInspirations.FindAsync(id);
        if (item is null) return false;
        db.LifeInspirations.Remove(item);
        return await db.SaveChangesAsync() > 0;
    }
}
