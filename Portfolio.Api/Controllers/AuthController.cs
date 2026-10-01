using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Dto;
using Portfolio.Api.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }
}
