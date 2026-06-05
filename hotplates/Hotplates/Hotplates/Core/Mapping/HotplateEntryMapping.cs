using Hotplates.Core.Database.Enums;
using Hotplates.Core.Database.Models;
using Hotplates.Core.DTOs;

namespace Hotplates.Core.Mapping;

public static class HotplateEntryMapping
{
    public static HotplateEntryDto ToDto(this HotplateEntry entity)
    {
        return new HotplateEntryDto(
            Id: entity.Id,
            Plate: entity.Plate,
            State: entity.State,
            Description: entity.Description,
            Category: entity.Category.ToString(),
            Source: entity.Source.ToString(),
            CreatedAt: entity.CreatedAt,
            ExpiresAt: entity.ExpiresAt,
            Severity: entity.Severity,
            Status: entity.Status,
            LastSeenTimestamp: entity.LastSeenTimestamp,
            LastSeenLatitude: entity.LastSeenLatitude,
            LastSeenLongitude: entity.LastSeenLongitude,
            LastSeenDeviceId: entity.LastSeenDeviceId
        );
    }

    public static HotplateEntry ToEntity(this HotplateCreateRequest request)
    {
        return new HotplateEntry
        {
            Id = Guid.NewGuid(),
            Plate = request.Plate.Trim().ToUpperInvariant(),
            State = request.State?.Trim().ToUpperInvariant(),
            Description = request.Description,
            Category = Enum.Parse<HotplateCategory>(request.Category, ignoreCase: true),
            Source = Enum.Parse<HotplateSource>(request.Source, ignoreCase: true),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = request.ExpiresAt,
            Severity = request.Severity,
            Status = request.Status
        };
    }

    public static void ApplyUpdate(this HotplateEntry entity, HotplateUpdateRequest request)
    {
        entity.Description = request.Description;
        entity.Category = Enum.Parse<HotplateCategory>(request.Category, ignoreCase: true);
        entity.Source = Enum.Parse<HotplateSource>(request.Source, ignoreCase: true);
        entity.ExpiresAt = request.ExpiresAt;
        entity.Severity = request.Severity;
    }
}
