using Microsoft.AspNetCore.Mvc;
using alpr.api.Database.Models;

namespace alpr.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private static readonly List<Video> _videos = new();
    private static int _nextId = 1;
    private static readonly object _lock = new();

    [HttpPost]
    public async Task<ActionResult<Video>> UploadVideoAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

        var fileName = Path.GetFileName(file.FileName);
        var uniqueName = $"{Guid.NewGuid():N}_{fileName}";
        var filePath = Path.Combine(uploadsDir, uniqueName);

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        Video video;
        lock (_lock)
        {
            video = new Video
            {
                Id = _nextId++,
                FileName = fileName,
                FilePath = filePath,
                ContentType = file.ContentType ?? "application/octet-stream",
                UploadTime = DateTime.UtcNow,
                ProcessingStatus = "Uploaded"
            };
            _videos.Add(video);
        }

        return CreatedAtAction(nameof(GetById), new { id = video.Id }, video);
    }

    /// <summary>
    /// Retrieves a collection of all available videos.
    /// </summary>
    /// <returns>An <see cref="ActionResult{T}"/> containing an enumerable collection of <see cref="Video"/> objects representing
    /// all videos. The collection will be empty if no videos are available.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Video>> GetVideosList()
    {
        return Ok(_videos.Select(v => new Video
        {
            Id = v.Id,
            FileName = v.FileName,
            UploadTime = v.UploadTime,
            ProcessingStatus = v.ProcessingStatus,
            ContentType = v.ContentType,
            FilePath = v.FilePath
        }));
    }

    /// <summary>
    /// Gets a video file by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns the file content if found, otherwise returns 404 Not Found</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var video = _videos.FirstOrDefault(v => v.Id == id);
        if (video == null) return NotFound();
        if (!System.IO.File.Exists(video.FilePath)) return NotFound("File not found on disk.");

        var stream = System.IO.File.OpenRead(video.FilePath);
        return File(stream, video.ContentType, video.FileName);
    }
}
