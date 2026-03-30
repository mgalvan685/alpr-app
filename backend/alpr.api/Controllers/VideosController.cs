using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.DTOs;
using alpr.api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace alpr.api.Controllers;

[Route("[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly AlprDbContext _db;
    //private readonly ILogger<VideosController> _logger;
    private static readonly List<Video> _videos = new();
    private static int _nextId = 1;
    private static readonly object _lock = new();

    public VideosController(AlprDbContext db)//, ILogger<VideosController> logger)
    {
        _db = db;
        //_logger = logger;
    }

    #region POST
    [HttpPost("upload")]
    public async Task<ActionResult<VideoDto>> UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var video = new Video
        {
            FileName = file.FileName,
            UploadTime = DateTime.UtcNow,
            ProcessingStatus = VideoProcessingStatus.PENDING
        };

        _db.Videos.Add(video);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = video.Id },
            new VideoDto(
                video.Id,
                video.FileName,
                video.UploadTime,
                video.ProcessingStatus
            ));
    }
    #endregion

    #region GET
    /// <summary>
    /// Retrieves a collection of all available videos.
    /// </summary>
    /// <returns>An <see cref="ActionResult{T}"/> containing an enumerable collection of <see cref="Video"/> objects representing
    /// all videos. The collection will be empty if no videos are available.</returns>
    [HttpGet]
    public IEnumerable<VideoDto> GetAll()
    {
        return _db.Videos
            .Select(v => new VideoDto(
                v.Id,
                v.FileName,
                v.UploadTime,
                v.ProcessingStatus
            ))
            .ToList();
    }

    /// <summary>
    /// Gets a video file by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns the file content if found, otherwise returns 404 Not Found</returns>
    [HttpGet("{id:int}")]
    public ActionResult<VideoDto> GetById(int id)
    {
        var video = _db.Videos.Find(id);

        if (video == null)
            return NotFound();

        return new VideoDto(
            video.Id,
            video.FileName,
            video.UploadTime,
            video.ProcessingStatus
        );
    }
    #endregion
}
