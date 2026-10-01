namespace Portfolio.Api.Models.Dto;

public record LoginRequestDto(string Username, string Password);

public record LoginResponseDto(string AccessToken, DateTime ExpiresAtUtc);
