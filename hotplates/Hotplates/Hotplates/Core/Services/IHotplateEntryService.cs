using Hotplates.Core.DTOs;

namespace Hotplates.Core.Services;

public interface IHotplateEntryService
{
    Task<HotplateEntryDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<HotplateEntryDto>> GetAllAsync();
    Task<HotplateEntryDto> CreateAsync(HotplateCreateRequest request);
    Task<HotplateEntryDto?> UpdateAsync(Guid id, HotplateUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
}
