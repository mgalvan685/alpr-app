namespace Hotplates.Core.DTOs;

public record HotplateEntryDto(
    Guid Id,
    string Plate,
    string? State,
    string Description,
    string Category,
    string Source,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    int Severity,
    string Status,
    DateTime? LastSeenTimestamp,
    double? LastSeenLatitude,
    double? LastSeenLongitude,
    string? LastSeenDeviceId);