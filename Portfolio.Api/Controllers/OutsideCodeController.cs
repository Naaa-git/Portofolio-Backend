using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/outside-code")]
public class OutsideCodeController(IOutsideCodeService service) : ControllerBase
{
    // --- Intro ---

    [HttpGet("intro")]
    public async Task<ActionResult<OutsideCodeIntroDto>> GetIntro([FromQuery] string lang = "id")
    {
        var intro = await service.GetIntroAsync(lang);
        return intro is null ? NotFound() : Ok(intro);
    }

    [HttpGet("intro/admin")]
    [Authorize]
    public async Task<ActionResult<OutsideCodeIntroAdminDto>> GetIntroAdmin()
    {
        var intro = await service.GetIntroAdminAsync();
        return intro is null ? NotFound() : Ok(intro);
    }

    [HttpPut("intro")]
    [Authorize]
    public async Task<ActionResult<OutsideCodeIntroAdminDto>> UpdateIntro(OutsideCodeIntroUpsertDto dto) =>
        Ok(await service.UpdateIntroAsync(dto));

    // --- Away From the Keyboard ---

    [HttpGet("away-from-keyboard")]
    public async Task<ActionResult<List<AwayFromKeyboardItemDto>>> GetAwayFromKeyboard([FromQuery] string lang = "id") =>
        Ok(await service.GetAwayFromKeyboardAsync(lang));

    [HttpGet("away-from-keyboard/admin")]
    [Authorize]
    public async Task<ActionResult<List<AwayFromKeyboardItemAdminDto>>> GetAwayFromKeyboardAdmin() =>
        Ok(await service.GetAwayFromKeyboardAdminAsync());

    [HttpPost("away-from-keyboard")]
    [Authorize]
    public async Task<ActionResult<AwayFromKeyboardItemAdminDto>> AddAwayFromKeyboard(AwayFromKeyboardItemUpsertDto dto) =>
        Ok(await service.AddAwayFromKeyboardAsync(dto));

    [HttpPut("away-from-keyboard/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateAwayFromKeyboard(int id, AwayFromKeyboardItemUpsertDto dto) =>
        await service.UpdateAwayFromKeyboardAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("away-from-keyboard/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAwayFromKeyboard(int id) =>
        await service.DeleteAwayFromKeyboardAsync(id) ? NoContent() : NotFound();

    // --- Movies & Shows ---

    [HttpGet("movies")]
    public async Task<ActionResult<List<MovieTakeDto>>> GetMovies([FromQuery] string lang = "id") =>
        Ok(await service.GetMoviesAsync(lang));

    [HttpGet("movies/admin")]
    [Authorize]
    public async Task<ActionResult<List<MovieTakeAdminDto>>> GetMoviesAdmin() =>
        Ok(await service.GetMoviesAdminAsync());

    [HttpPost("movies")]
    [Authorize]
    public async Task<ActionResult<MovieTakeAdminDto>> AddMovie(MovieTakeUpsertDto dto) =>
        Ok(await service.AddMovieAsync(dto));

    [HttpPut("movies/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateMovie(int id, MovieTakeUpsertDto dto) =>
        await service.UpdateMovieAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("movies/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteMovie(int id) =>
        await service.DeleteMovieAsync(id) ? NoContent() : NotFound();

    // --- Music Artists ---

    [HttpGet("music")]
    public async Task<ActionResult<List<MusicArtistDto>>> GetMusicArtists() =>
        Ok(await service.GetMusicArtistsAsync());

    [HttpPost("music")]
    [Authorize]
    public async Task<ActionResult<MusicArtistDto>> AddMusicArtist(MusicArtistUpsertDto dto) =>
        Ok(await service.AddMusicArtistAsync(dto));

    [HttpPut("music/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateMusicArtist(int id, MusicArtistUpsertDto dto) =>
        await service.UpdateMusicArtistAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("music/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteMusicArtist(int id) =>
        await service.DeleteMusicArtistAsync(id) ? NoContent() : NotFound();

    // --- Podcasts ---

    [HttpGet("podcasts")]
    public async Task<ActionResult<List<PodcastChannelDto>>> GetPodcasts() =>
        Ok(await service.GetPodcastsAsync());

    [HttpPost("podcasts")]
    [Authorize]
    public async Task<ActionResult<PodcastChannelDto>> AddPodcast(PodcastChannelUpsertDto dto) =>
        Ok(await service.AddPodcastAsync(dto));

    [HttpPut("podcasts/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdatePodcast(int id, PodcastChannelUpsertDto dto) =>
        await service.UpdatePodcastAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("podcasts/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeletePodcast(int id) =>
        await service.DeletePodcastAsync(id) ? NoContent() : NotFound();

    // --- Books ---

    [HttpGet("books")]
    public async Task<ActionResult<List<OutsideCodeBookDto>>> GetBooks([FromQuery] string lang = "id") =>
        Ok(await service.GetBooksAsync(lang));

    [HttpGet("books/admin")]
    [Authorize]
    public async Task<ActionResult<List<OutsideCodeBookAdminDto>>> GetBooksAdmin() =>
        Ok(await service.GetBooksAdminAsync());

    [HttpPost("books")]
    [Authorize]
    public async Task<ActionResult<OutsideCodeBookAdminDto>> AddBook(OutsideCodeBookUpsertDto dto) =>
        Ok(await service.AddBookAsync(dto));

    [HttpPut("books/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateBook(int id, OutsideCodeBookUpsertDto dto) =>
        await service.UpdateBookAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("books/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteBook(int id) =>
        await service.DeleteBookAsync(id) ? NoContent() : NotFound();

    // --- Life Inspirations ---

    [HttpGet("life-inspirations")]
    public async Task<ActionResult<List<LifeInspirationDto>>> GetLifeInspirations([FromQuery] string lang = "id") =>
        Ok(await service.GetLifeInspirationsAsync(lang));

    [HttpGet("life-inspirations/admin")]
    [Authorize]
    public async Task<ActionResult<List<LifeInspirationAdminDto>>> GetLifeInspirationsAdmin() =>
        Ok(await service.GetLifeInspirationsAdminAsync());

    [HttpPost("life-inspirations")]
    [Authorize]
    public async Task<ActionResult<LifeInspirationAdminDto>> AddLifeInspiration(LifeInspirationUpsertDto dto) =>
        Ok(await service.AddLifeInspirationAsync(dto));

    [HttpPut("life-inspirations/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateLifeInspiration(int id, LifeInspirationUpsertDto dto) =>
        await service.UpdateLifeInspirationAsync(id, dto) ? NoContent() : NotFound();

    [HttpDelete("life-inspirations/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteLifeInspiration(int id) =>
        await service.DeleteLifeInspirationAsync(id) ? NoContent() : NotFound();
}
