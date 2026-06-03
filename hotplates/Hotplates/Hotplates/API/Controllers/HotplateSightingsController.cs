using Hotplates.Core.DTOs;
using Hotplates.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotplates.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotplateSightingsController : ControllerBase
{
    private readonly IHotplateSightingService _service;

    public HotplateSightingsController(IHotplateSightingService service)
    {
        _service = service;
    }

    // POST: api/hotplatesightings
    [HttpPost]
    [ProducesResponseType(typeof(HotplateSightingDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> LogSighting([FromBody] HotplateSightingCreateRequest request)
    {
        var sighting = await _service.LogSightingAsync(request);
        return CreatedAtAction(nameof(GetRecent), new { }, sighting);
    }

    // GET: api/hotplatesightings/recent?count=50
    [HttpGet("recent")]
    [ProducesResponseType(typeof(IEnumerable<HotplateSightingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecent([FromQuery] int count = 50)
    {
        var sightings = await _service.GetRecentSightingsAsync(count);
        return Ok(sightings);
    }

    // GET: api/hotplatesightings/plate/{plate}
    [HttpGet("plate/{plate}")]
    [ProducesResponseType(typeof(IEnumerable<HotplateSightingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForPlate(string plate)
    {
        var sightings = await _service.GetSightingsForPlateAsync(plate);
        return Ok(sightings);
    }
}
