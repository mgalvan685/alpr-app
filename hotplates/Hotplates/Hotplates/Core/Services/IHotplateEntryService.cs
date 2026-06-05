using Hotplates.Core.DTOs;

namespace Hotplates.Core.Services;

public interface IHotplateEntryService
{
    Task<HotplateEntryDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<HotplateEntryDto>> GetAllAsync(bool includeDisabled = false);
    Task<HotplateEntryDto> CreateAsync(HotplateCreateRequest request);
    Task<HotplateEntryDto?> UpdateAsync(Guid id, HotplateUpdateRequest request);
    Task<bool> ReactivateAsync(Guid id);
    Task<bool> MarkInactiveAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
