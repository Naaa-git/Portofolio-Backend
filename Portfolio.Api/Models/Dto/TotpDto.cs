using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Dto;

public record TotpSetupResponseDto(string Secret, string QrCodeImageBase64);

public record TotpVerifyRequestDto([Required, RegularExpression(@"^\d{6}$")] string Code);
