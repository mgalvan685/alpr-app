using Hotplates.Core.DTOs;

namespace Hotplates.Core.Services;

public interface IHotplateSightingService
{
    Task<HotplateSightingDto> LogSightingAsync(HotplateSightingCreateRequest request);
    Task<IEnumerable<HotplateSightingDto>> GetRecentSightingsAsync(int count = 50);
    Task<IEnumerable<HotplateSightingDto>> GetSightingsForPlateAsync(string plate);
}
