using Microsoft.AspNetCore.Mvc;
using alpr.api.Database.Models;

namespace alpr.api.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatesController : ControllerBase
{
    // In-memory store; in a real app this would be a database/service
    private static readonly List<PlateSighting> _sightings = new();
    private static readonly object _lock = new();

    // For demonstration: an endpoint to add a sighting (not requested but useful for testing)
    [HttpPost]
    public ActionResult<PlateSighting> Add([FromBody] PlateSighting sighting)
    {
        if (sighting == null || string.IsNullOrWhiteSpace(sighting.Plate)) return BadRequest();
        lock (_lock)
        {
            sighting.Id = _sightings.Count > 0 ? _sightings.Max(s => s.Id) + 1 : 1;
            sighting.Timestamp = sighting.Timestamp == default ? DateTime.UtcNow : sighting.Timestamp;
            _sightings.Add(sighting);
        }
        return CreatedAtAction(nameof(GetById), new { id = sighting.Id }, sighting);
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlateSighting>> GetAllSightings()
    {
        return Ok(_sightings.OrderByDescending(s => s.Timestamp));
    }

    [HttpGet("summaries")]
    public ActionResult<IEnumerable<PlateSummary>> GetSummaries()
    {
        var summaries = _sightings
            .GroupBy(s => s.Plate)
            .Select(g => new PlateSummary
            {
                Plate = g.Key,
                TotalCount = g.Count(),
                LastSeen = g.Max(x => x.Timestamp)
            })
            .OrderByDescending(p => p.LastSeen)
            .ToList();

        return Ok(summaries);
    }

    [HttpGet("byplate/{plate}")]
    public ActionResult<PlateSummary> GetByPlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate)) return BadRequest();

        var group = _sightings.Where(s => string.Equals(s.Plate, plate, StringComparison.OrdinalIgnoreCase));
        if (!group.Any()) return NotFound();

        var summary = new PlateSummary
        {
            Plate = plate,
            TotalCount = group.Count(),
            LastSeen = group.Max(s => s.Timestamp)
        };
        return Ok(summary);
    }

    [HttpGet("{id}")]
    public ActionResult<PlateSighting> GetById(int id)
    {
        var sighting = _sightings.FirstOrDefault(s => s.Id == id);
        if (sighting == null) return NotFound();
        return Ok(sighting);
    }
}
