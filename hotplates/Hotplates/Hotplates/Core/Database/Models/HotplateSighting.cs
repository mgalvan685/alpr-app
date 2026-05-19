namespace Hotplates.Core.Database.Models;

public class HotplateSighting
{
    public Guid Id { get; set; }

    public Guid HotplateEntryId { get; set; }
    public HotplateEntry HotplateEntry { get; set; } = default!;

    public string Plate { get; set; } = default!;
    public string? State { get; set; }

    public DateTime Timestamp { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public float Confidence { get; set; }

    public string? ImageUrl { get; set; }
    public string Source { get; set; } = default!;
    public string? DeviceId { get; set; }

    public string? RawMetadataJson { get; set; }
}
