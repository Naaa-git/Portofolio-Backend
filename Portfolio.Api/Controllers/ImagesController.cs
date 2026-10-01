using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/images")]
[Authorize]
public class ImagesController(IImageStorageService storage) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ActionResult<object>> Upload(IFormFile file)
    {
        try
        {
            var url = await storage.UploadAsync(file);
            return Ok(new { url });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
