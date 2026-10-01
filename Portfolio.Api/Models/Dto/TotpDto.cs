namespace Portfolio.Api.Models.Dto;

public record TotpSetupResponseDto(string Secret, string QrCodeImageBase64);

public record TotpVerifyRequestDto(string Code);
