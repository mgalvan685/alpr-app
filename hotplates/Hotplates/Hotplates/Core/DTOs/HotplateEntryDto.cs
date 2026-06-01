using System.ComponentModel.DataAnnotations;

namespace Hotplates.Core.DTOs;

public record HotplateEntryDto {
    public required Guid Id { get; init; }
    public required string Plate { get; init; }
    public string? State { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Source { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; init; }
    public required int Severity { get; init; }
    public DateTime? LastSeenTimestamp { get; init; }
    public double? LastSeenLatitude { get; init; }
    public double? LastSeenLongitude { get; init; }
    public string? LastSeenDeviceId { get; init; }
}