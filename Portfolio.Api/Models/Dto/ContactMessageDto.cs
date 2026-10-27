using System.ComponentModel.DataAnnotations;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Models.Dto;

public record ContactMessageSubmitDto(
    [Required, StringLength(100, MinimumLength = 1)] string Name,
    [Required, EmailAddress, StringLength(254)] string Email,
    [Required, StringLength(2000, MinimumLength = 5)] string Message);

public record ContactMessageAdminDto(
    int Id,
    string Name,
    string Email,
    string Message,
    ContactMessageStatus Status,
    DateTime CreatedAtUtc,
    DateTime? SentAtUtc,
    string? ErrorMessage);
