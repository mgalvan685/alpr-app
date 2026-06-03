using Hotplates.Core.Database.Enums;
using Hotplates.Core.Database.Models;
using Hotplates.Core.DTOs;

namespace Hotplates.Core.Mapping;

public static class HotplateSightingMapping
{
    public static HotplateSightingDto ToDto(this HotplateSighting entity)
    {
        return new HotplateSightingDto(
            Id: entity.Id,
            HotplateEntryId: entity.HotplateEntryId,
            Plate: entity.Plate,
            State: entity.State,
            Timestamp: entity.Timestamp,
            Latitude: entity.Latitude,
            Longitude: entity.Longitude,
            Confidence: entity.Confidence,
            ImageUrl: entity.ImageUrl,
            Source: entity.Source.ToString(),
            DeviceId: entity.DeviceId,
            RawMetadataJson: entity.RawMetadataJson
        );
    }

    public static HotplateSighting ToEntity(this HotplateSightingCreateRequest request)
    {
        return new HotplateSighting
        {
            Id = Guid.NewGuid(),
            Plate = request.Plate.Trim().ToUpperInvariant(),
            State = request.State?.Trim().ToUpperInvariant(),
            Timestamp = request.Timestamp,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Confidence = request.Confidence,
            ImageUrl = request.ImageUrl,
            Source = Enum.Parse<HotplateSource>(request.Source, ignoreCase: true).ToString(),
            DeviceId = request.DeviceId,
            RawMetadataJson = request.RawMetadataJson
        };
    }
}
