namespace Hotplates.Core.DTOs;

/// <summary>
/// Used when a device or service reports a new sighting of a plate.
/// </summary>
public record HotplateSightingCreateRequest(
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