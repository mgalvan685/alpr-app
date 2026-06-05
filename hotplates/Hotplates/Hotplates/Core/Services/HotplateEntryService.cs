using Hotplates.Core.Database;
using Hotplates.Core.DTOs;
using Hotplates.Core.Mapping;
using Hotplates.Helpers;
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

    public async Task<IEnumerable<HotplateEntryDto>> GetAllAsync(bool includeDisabled = false)
    {
        var query = _db.HotplateEntries.AsQueryable();

        if (!includeDisabled)
        {
            query = query.Where(x => x.Status == HotplateStatus.Active);
        }

        return await query
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

    public async Task<bool> ReactivateAsync(Guid id)
    {
        var entity = await _db.HotplateEntries.FindAsync(id);
        if (entity == null)
            return false;
        
        entity.Status = HotplateStatus.Active;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkInactiveAsync(Guid id)
    {
        var entity = await _db.HotplateEntries.FindAsync(id);
        if (entity == null)
            return false;

        entity.Status = HotplateStatus.Inactive;
        await _db.SaveChangesAsync();
        return true;

    }

    // TODO: Add permissions to this so it's not available to all users
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
