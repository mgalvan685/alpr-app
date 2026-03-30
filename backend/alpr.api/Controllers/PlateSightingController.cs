using alpr.api.Database;
using alpr.api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace alpr.api.Controllers;

[ApiController]
[Route("platesightings")]
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
    [HttpGet]
    public IEnumerable<PlateSightingDto> GetAll()
    {
        return _db.PlateSightings
            .Select(s => new PlateSightingDto(
                s.Id,
                s.Plate,
                s.Timestamp,
                s.VideoId,
                s.FrameNumber,
                s.Confidence
            ))
            .ToList();
    }

    [HttpGet("video/{videoId:int}")]
    public IEnumerable<PlateSightingDto> GetByVideo(int videoId)
    {
        return _db.PlateSightings
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
    }
    #endregion
}