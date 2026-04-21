using alpr.api.Database;
using alpr.api.DTOs;
using alpr.api.Shared;
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
    public async Task<ActionResult<IEnumerable<PlateSightingDto>>> GetAll()
    {
        var sightings = await _db.PlateSightings
            .OrderByDescending(s => s.Timestamp)
            .Select(s => new PlateSightingDto
            {
                Id = s.Id,
                Plate = s.Plate,
                IssueState = s.IssueState,
                Timestamp = s.Timestamp,
                VideoId = s.VideoId,
                FrameNumber = s.FrameNumber,
                Confidence = s.Confidence,
                FrameUrl = s.FrameUrl,
                BoundingBox = new BoundingBox
                {
                    X = s.BoundingBox.X,
                    Y = s.BoundingBox.Y,
                    Width = s.BoundingBox.Width,
                    Height = s.BoundingBox.Height
                }
            })
            .ToListAsync();

        return Ok(sightings);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("video/{videoId:int}")]
    public async Task<ActionResult<IEnumerable<PlateSightingDto>>> GetByVideo(int videoId)
    {
        var sightings = await _db.PlateSightings
            .Where(s => s.VideoId == videoId)
            .OrderBy(s => s.FrameNumber)
            .Select(s => new PlateSightingDto
            {
                Id = s.Id,
                Plate = s.Plate,
                IssueState = s.IssueState,
                Timestamp = s.Timestamp,
                VideoId = s.VideoId,
                FrameNumber = s.FrameNumber,
                Confidence = s.Confidence,
                FrameUrl = s.FrameUrl,
                BoundingBox = new BoundingBox
                {
                    X = s.BoundingBox.X,
                    Y = s.BoundingBox.Y,
                    Width = s.BoundingBox.Width,
                    Height = s.BoundingBox.Height
                }
            })
            .ToListAsync();

        return Ok(sightings);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("plate/{plate}")]
    public async Task<ActionResult<IEnumerable<PlateSightingDto>>> GetByPlate(string plate)
    {
        var sightings = await _db.PlateSightings
            .Where(s => s.Plate == plate)
            .OrderByDescending(s => s.Timestamp)
            .Select(s => new PlateSightingDto
            {
                Id = s.Id,
                Plate = s.Plate,
                IssueState = s.IssueState,
                Timestamp = s.Timestamp,
                VideoId = s.VideoId,
                FrameNumber = s.FrameNumber,
                Confidence = s.Confidence,
                FrameUrl = s.FrameUrl,
                BoundingBox = new BoundingBox
                {
                    X = s.BoundingBox.X,
                    Y = s.BoundingBox.Y,
                    Width = s.BoundingBox.Width,
                    Height = s.BoundingBox.Height
                }
            })
            .ToListAsync();

        return Ok(sightings);
    }

    #endregion
}