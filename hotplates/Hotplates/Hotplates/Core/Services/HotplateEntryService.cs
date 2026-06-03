using Hotplates.Core.Database;
using Hotplates.Core.DTOs;
using Hotplates.Core.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Hotplates.Core.Services;

public class HotplateEntryService : IHotplateEntryService
{
    private readonly HotplatesDbContext _db;

    public HotplateEntryService(HotplatesDbContext db)
    {
        _db = db;
    }

    public async Task<HotplateEntryDto?> GetByIdAsync(Guid id)
    {
        var entity = await _db.HotplateEntries.FindAsync(id);
        return entity?.ToDto();
    }

    public async Task<IEnumerable<HotplateEntryDto>> GetAllAsync()
    {
        return await _db.HotplateEntries
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    public async Task<HotplateEntryDto> CreateAsync(HotplateCreateRequest request)
    {
        var entity = request.ToEntity();

        _db.HotplateEntries.Add(entity);
        await _db.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<HotplateEntryDto?> UpdateAsync(Guid id, HotplateUpdateRequest request)
    {
        var entity = await _db.HotplateEntries.FindAsync(id);
        if (entity == null)
            return null;

        entity.ApplyUpdate(request);
        await _db.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.HotplateEntries.FindAsync(id);
        if (entity == null)
            return false;

        _db.HotplateEntries.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
