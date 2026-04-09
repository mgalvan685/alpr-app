using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.DTOs;
using alpr.api.Helpers;
using alpr.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly AlprDbContext _db;
    private readonly ILogger<VideosController> _logger;
    private readonly IVideoMetadataService _metadataService;

    private static readonly List<Video> _videos = new();
    private static int _nextId = 1;
    private static readonly object _lock = new();

    public VideosController(AlprDbContext db, ILogger<VideosController> logger, IVideoMetadataService metadataService)
    {
        _db = db;
        _logger = logger;
        _metadataService = metadataService;
    }

    #region POST
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("upload")]
    public async Task<ActionResult<VideoDto>> UploadAsync(IFormFile file)
    {
        var errorMessage = string.Empty;

        if (!IsValidVideo(file, out errorMessage))
            return BadRequest(errorMessage);

        // TODO: Move storage to a cloud system like Azure Blob Storage or AWS. For now, for the sake of time, local storage is used.
        var storagePath = Path.Combine("storage", "videos");
        Directory.CreateDirectory(storagePath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(storagePath, fileName);

        try
        {
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save uploaded file.");
            return StatusCode(500, "Failed to save file to server.");
        }

        var metadata = await _metadataService.ExtractAsync(fullPath);

        var video = new Video
        {
            FileName = file.FileName,
            FilePath = fullPath,
            UploadTime = DateTime.UtcNow,
            ProcessingStatus = VideoProcessingStatus.PENDING,
            Metadata = metadata == null ? null : new VideoMetadata
            {
                DurationSeconds = metadata.DurationSeconds,
                Width = metadata.Width,
                Height = metadata.Height,
                FrameRate = metadata.FrameRate
            }
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public ActionResult<VideoDto> GetAll()
    {
        var videos = _db.Videos
            .AsNoTracking()
            .Select(v => new VideoDto(
                v.Id,
                v.FileName,
                v.UploadTime,
                v.ProcessingStatus
            ))
            .ToList();

        return Ok(videos);
    }

    /// <summary>
    /// Gets a video file by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns the file content if found, otherwise returns 404 Not Found</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Downloads the video file associated with the specified ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If the video or its file is not found, returns a 404 Not Found response</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:int}/download")]
    public IActionResult Download(int id)
    {
        // TODO: Move storage to a cloud system like Azure Blob Storage or AWS. For now, for the sake of time, local storage is used.

        var video = _db.Videos.Find(id);

        if (video == null)
            return NotFound("Video not found.");

        if (!System.IO.File.Exists(video.FilePath))
            return NotFound("Video file missing on server.");

        var fileBytes = System.IO.File.ReadAllBytes(video.FilePath);

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(video.FilePath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return File(fileBytes, contentType, video.FileName);
    }

    [HttpGet("{id:int}/metadata")]
    [ProducesResponseType(typeof(VideoMetadataDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMetadata(int id)
    {
        var metadata = await _db.VideoMetadata
            .FirstOrDefaultAsync(m => m.VideoId == id);

        if (metadata == null)
            return NotFound();

        return Ok(new VideoMetadataDto(metadata));
    }
    #endregion

    #region DELETE
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var video = await _db.Videos.FindAsync(id);

        if (video == null)
            return NotFound("Video not found.");

        // Try to delete the file if it exists
        try
        {
            if (System.IO.File.Exists(video.FilePath))
            {
                System.IO.File.Delete(video.FilePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete video file at path: {Path}", video.FilePath);
            return StatusCode(500, "Failed to delete video file from server.");
        }

        // Remove DB record
        _db.Videos.Remove(video);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete video record from database.");
            return StatusCode(500, "Failed to delete video record from database.");
        }

        return NoContent();
    }
    #endregion

    #region Validations
    private bool IsValidVideo(IFormFile file, out string errorMessage)
    {
        if (file == null || file.Length == 0)
        {
            errorMessage = "No file uploaded.";
            return false;
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!VideoConstants.ALLOWED_EXTENSIONS.Contains(ext))
        {
            errorMessage = $"Unsupported file type. Allowed: {string.Join(", ", VideoConstants.ALLOWED_EXTENSIONS)}.";
            return false;
        }

        if (file.Length > VideoConstants.MAX_FILE_SIZE)
        {
            errorMessage = $"File too large. Max allowed size is {VideoConstants.MAX_FILE_SIZE / (1024 * 1024)} MB.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
    #endregion
}
