using Portfolio.Api.Models.Dto;

namespace Portfolio.Api.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}
