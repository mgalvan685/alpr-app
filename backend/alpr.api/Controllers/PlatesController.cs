using alpr.api.Database;
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
    public async Task<ActionResult<IEnumerable<PlateSummaryDto>>> GetAll()
    {
        var summaries = await _db.PlateSummaries
            .OrderByDescending(p => p.LastSeen)
            .Select(p => new PlateSummaryDto
            {
                Plate = p.Plate,
                IssueState = p.IssueState,
                TotalCount = p.TotalCount,
                LastSeen = p.LastSeen
            })
            .ToListAsync();

        return Ok(summaries);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{plate}")]
    public async Task<ActionResult<PlateSummaryDto>> GetOne(string plate)
    {
        var summary = await _db.PlateSummaries
            .Where(p => p.Plate == plate)
            .Select(p => new PlateSummaryDto
            {
                Plate = p.Plate,
                IssueState = p.IssueState,
                TotalCount = p.TotalCount,
                LastSeen = p.LastSeen
            })
            .FirstOrDefaultAsync();

        if (summary == null)
            return NotFound();

        return Ok(summary);
    }
}