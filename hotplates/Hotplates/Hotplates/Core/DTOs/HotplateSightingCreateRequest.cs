namespace Hotplates.Core.DTOs;

/// <summary>
/// Used when a device or service reports a new sighting of a plate.
/// </summary>
public record HotplateSightingCreateRequest
{
    public required string Plate { get; init; }
    public string? State { get; init; }
    public required DateTime Timestamp { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public required float Confidence { get; init; }
    public string? ImageUrl { get; init; }
    public required string Source { get; init; }
    public string? DeviceId { get; init; }
    public string? RawMetadataJson { get; init; }
}