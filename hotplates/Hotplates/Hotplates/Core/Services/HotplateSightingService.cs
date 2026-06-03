using Hotplates.Core.Database;
using Hotplates.Core.DTOs;
using Hotplates.Core.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Hotplates.Core.Services;

public class HotplateSightingService : IHotplateSightingService
{
    private readonly HotplatesDbContext _db;

    public HotplateSightingService(HotplatesDbContext db)
    {
        _db = db;
    }

    public async Task<HotplateSightingDto> LogSightingAsync(HotplateSightingCreateRequest request)
    {
        var entity = request.ToEntity();

        _db.HotplateSightings.Add(entity);
        await _db.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<IEnumerable<HotplateSightingDto>> GetRecentSightingsAsync(int count = 50)
    {
        return await _db.HotplateSightings
            .OrderByDescending(x => x.Timestamp)
            .Take(count)
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<HotplateSightingDto>> GetSightingsForPlateAsync(string plate)
    {
        var normalized = plate.Trim().ToUpperInvariant();

        return await _db.HotplateSightings
            .Where(x => x.Plate == normalized)
            .OrderByDescending(x => x.Timestamp)
            .Select(x => x.ToDto())
            .ToListAsync();
    }
}
