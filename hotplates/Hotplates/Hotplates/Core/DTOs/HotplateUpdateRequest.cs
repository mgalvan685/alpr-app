namespace Hotplates.Core.DTOs;

/// <summary>
/// Used when updating an existing hotplate entry.
/// </summary>
public record HotplateUpdateRequest
{
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Source { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public required int Severity { get; init; } = 0;
}