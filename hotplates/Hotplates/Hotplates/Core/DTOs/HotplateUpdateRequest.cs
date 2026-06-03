namespace Hotplates.Core.DTOs;

/// <summary>
/// Used when updating an existing hotplate entry.
/// </summary>
public record HotplateUpdateRequest(
    string Description,
    string Category,
    string Source,
    DateTime? ExpiresAt,
    int Severity);