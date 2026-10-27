using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Models.Events;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactMessagesController(
    IContactMessageService service,
    IContactEmailProducer producer,
    ILogger<ContactMessagesController> logger) : ControllerBase
{
    [HttpPost]
    [EnableRateLimiting("contact")]
    public async Task<IActionResult> Submit(ContactMessageSubmitDto dto)
    {
        var created = await service.SubmitAsync(dto);

        try
        {
            await producer.PublishAsync(new ContactEmailMessage(created.Id));
        }
        catch (Exception ex)
        {
            // Row sudah tersimpan sebagai Pending — nggak hilang, cuma belum
            // dikirim notifikasinya. Tetap balikin sukses ke pengirim.
            logger.LogError(ex, "Gagal publish contact email event untuk ContactMessage {Id}", created.Id);
        }

        return Accepted();
    }

    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<List<ContactMessageAdminDto>>> GetAllAdmin() =>
        Ok(await service.GetAllAdminAsync());

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
