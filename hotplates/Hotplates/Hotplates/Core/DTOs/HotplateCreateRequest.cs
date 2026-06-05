using Hotplates.Helpers;

namespace Hotplates.Core.DTOs;

/// <summary>
/// Used when a client wants to create a new hotplate entry in the system.
/// </summary>
public record HotplateCreateRequest(
    string Plate,
    string? State,
    string Description,
    string Category,
    string Source,
    DateTime? ExpiresAt,
    int Severity,
    string Status = HotplateStatus.Active);