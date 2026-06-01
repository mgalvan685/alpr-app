namespace Hotplates.Core.DTOs;

/// <summary>
/// Used when a client wants to create a new hotplate entry in the system.
/// </summary>
public record HotplateCreateRequest
{
    public required string Plate { get; init; }
    public string? State { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Source { get; init; }
    public DateTime? ExpiresAt { get; init; } = default(DateTime?);
    public required int Severity { get; init; }
}