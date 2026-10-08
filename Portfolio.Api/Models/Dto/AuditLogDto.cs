namespace Portfolio.Api.Models.Dto;

public record AuditLogDto(
    int Id,
    DateTime TimestampUtc,
    string? AdminUsername,
    string Action,
    string EntityName,
    string EntityId,
    string? Changes);
