using alpr.api.Database;
using alpr.api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlateSightingController : ControllerBase
{
    private readonly AlprDbContext _db;
    //private readonly Logger<PlateSightingController> _logger;

    public PlateSightingController(AlprDbContext db)//, Logger<PlateSightingController> logger)
    {
        _db = db;
        //_logger = logger;
    }

    #region GET
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public ActionResult<IEnumerable<PlateSightingDto>> GetAll()
    {
        var plateSightings = _db.PlateSightings
            .AsNoTracking()
            .Select(s => new PlateSightingDto(
                s.Id,
                s.Plate,
                s.Timestamp,
                s.VideoId,
                s.FrameNumber,
                s.Confidence
            ))
            .ToList();

        return Ok(plateSightings);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("video/{videoId:int}")]
    public ActionResult<IEnumerable<PlateSightingDto>> GetByVideo(int videoId)
    {
        var plateSightings = _db.PlateSightings
            .AsNoTracking()
            .Where(s => s.VideoId == videoId)
            .Select(s => new PlateSightingDto(
                s.Id,
                s.Plate,
                s.Timestamp,
                s.VideoId,
                s.FrameNumber,
                s.Confidence
            ))
            .ToList();

        return Ok(plateSightings);
    }
    #endregion
}