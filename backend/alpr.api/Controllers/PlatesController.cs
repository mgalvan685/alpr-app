using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatesController : ControllerBase
{
    private readonly AlprDbContext _db;

    public PlatesController(AlprDbContext db)
    {
        _db = db;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlateSightingDto>>> GetAllSightings()
    {
        var sightings = await _db.PlateSightings
            .AsNoTracking()
            .Select(s => new PlateSightingDto(
                s.Id,
                s.Plate,
                s.Timestamp,
                s.VideoId,
                s.FrameNumber,
                s.Confidence
            ))
            .ToListAsync();

        return Ok(sightings);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("summaries")]
    public async Task<ActionResult<IEnumerable<PlateSummaryDto>>> GetSummaries()
    {
        var summaries = await _db.PlateSightings
            .AsNoTracking()
            .GroupBy(s => s.Plate)
            .Select(g => new PlateSummaryDto(
                g.Key,
                "", // TODO: Fix state lookup
                g.Count(),
                g.Max(x => x.Timestamp)
            ))
            .OrderByDescending(s => s.LastSeen)
            .ToListAsync();

        return Ok(summaries);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("byplate/{plate}")]
    public async Task<ActionResult<PlateSummaryDto>> GetByPlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
            return BadRequest();

        var group = await _db.PlateSightings
            .AsNoTracking()
            .Where(s => s.Plate.ToLower() == plate.ToLower())
            .ToListAsync();

        if (!group.Any())
            return NotFound();

        var summary = new PlateSummaryDto(
            plate,
            "", // TODO: Fix state lookup
            group.Count,
            group.Max(s => s.Timestamp)
        );

        return Ok(summary);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlateSightingDto>> GetById(int id)
    {
        var s = await _db.PlateSightings
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PlateSightingDto(
                x.Id,
                x.Plate,
                x.Timestamp,
                x.VideoId,
                x.FrameNumber,
                x.Confidence
            ))
            .FirstOrDefaultAsync();

        if (s == null)
            return NotFound();

        return Ok(s);
    }
}