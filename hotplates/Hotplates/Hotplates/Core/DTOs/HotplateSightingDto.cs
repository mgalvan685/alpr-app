namespace Hotplates.Core.DTOs;

public record HotplateSightingDto(
    Guid Id,
    Guid HotplateEntryId,
    string Plate,
    string? State,
    DateTime Timestamp,
    double Latitude,
    double Longitude,
    float Confidence,
    string? ImageUrl,
    string Source,
    string? DeviceId,
    string? RawMetadataJson);