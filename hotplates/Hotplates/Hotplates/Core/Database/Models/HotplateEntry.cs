using Hotplates.Core.Database.Enums;
using Hotplates.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Hotplates.Core.Database.Models;

public class HotplateEntry
{
    public Guid Id { get; set; }

    [MaxLength(DatabaseConstants.MAX_PLATE_LENGTH)]
    public string Plate { get; set; } = default!;
    public string? State { get; set; }

    public HotplateCategory Category { get; set; }
    public string Description { get; set; } = default!;
    public HotplateSource Source { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int Severity { get; set; }

    // Cached last-seen info
    public DateTime? LastSeenTimestamp { get; set; }
    public double? LastSeenLatitude { get; set; }
    public double? LastSeenLongitude { get; set; }
    public string? LastSeenDeviceId { get; set; }

    // Navigation
    public List<HotplateSighting> Sightings { get; set; } = new();
}
