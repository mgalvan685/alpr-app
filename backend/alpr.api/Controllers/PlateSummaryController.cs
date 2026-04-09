using Microsoft.AspNetCore.Mvc;
using alpr.api.Database;
using alpr.api.DTOs;

namespace alpr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlateSummaryController : ControllerBase
{
    private readonly AlprDbContext _db;
    //private readonly Logger<PlateSummaryController> _logger;

    public PlateSummaryController(AlprDbContext db)//, Logger<PlateSummaryController> logger)
    {
        _db = db;
        //_logger = logger;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public IEnumerable<PlateSummaryDto> GetAll()
    {
        return _db.PlateSummaries
            .Select(s => new PlateSummaryDto(
                s.Plate,
                s.State,
                s.TotalCount,
                s.LastSeen
            ))
            .ToList();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{plate}")]
    public ActionResult<PlateSummaryDto> GetByPlate(string plate)
    {
        var summary = _db.PlateSummaries.Find(plate);

        if (summary == null)
            return NotFound();

        return new PlateSummaryDto(
            summary.Plate,
            summary.State,
            summary.TotalCount,
            summary.LastSeen
        );
    }
}