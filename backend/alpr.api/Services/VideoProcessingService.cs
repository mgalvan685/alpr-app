using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.Helpers;
using alpr.api.Services.Helpers;
using alpr.api.Services.Interfaces;
using alpr.api.Shared;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Services;

public class VideoProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<VideoProcessingService> _logger;
    private readonly IAlprEngine _engine;

    public VideoProcessingService(
        IServiceProvider serviceProvider,
        ILogger<VideoProcessingService> logger,
        IAlprEngine engine)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _engine = engine;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AlprDbContext>();

            var video = await db.Videos
                .Where(v => v.ProcessingStatus == "Pending")
                .OrderBy(v => v.UploadTime)
                .FirstOrDefaultAsync(stoppingToken);

            if (video != null)
            {
                await ProcessVideoAsync(video, db, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessVideoAsync(Video video, AlprDbContext db, CancellationToken token)
    {
        try
        {
            _logger.LogInformation("Starting processing for video {VideoId}", video.Id);

            video.ProcessingStatus = "Processing";
            await db.SaveChangesAsync(token);

            // 1. Extract frames
            var frameDir = Path.Combine("frames", video.Id.ToString());
            Directory.CreateDirectory(frameDir);

            var frames = await FrameExtractor.ExtractFramesAsync(
                video.FilePath,
                frameDir,
                intervalMs: VideoConstants.FRAME_EXTRACTION_INTERVAL_MS);

            _logger.LogInformation("Extracted {Count} frames for video {VideoId}", frames.Count, video.Id);

            // 2. Run ALPR engine on the video
            var result = await _engine.ProcessVideoAsync(video.FilePath);

            _logger.LogInformation("Engine returned {Count} detections for video {VideoId}",
                result.Detections.Count, video.Id);

            // 3. Save detections
            foreach (var d in result.Detections)
            {
                var frameFileName = $"frame_{d.FrameNumber:D5}.jpg";
                var framePath = Path.Combine(frameDir, frameFileName);
                var frameUrl = $"/frames/{video.Id}/{frameFileName}";

                if (!File.Exists(framePath))
                {
                    _logger.LogWarning(
                        "Expected frame file {FramePath} for detection (plate {Plate}, frame {FrameNumber}) does not exist.",
                        framePath,
                        d.Plate,
                        d.FrameNumber);
                }

                var sighting = new PlateSighting
                {
                    Plate = d.Plate,
                    IssueState = "", // engine does not provide state yet
                    Timestamp = d.Timestamp,
                    VideoId = video.Id,
                    FrameNumber = d.FrameNumber,
                    Confidence = d.Confidence,
                    FrameUrl = frameUrl,
                    BoundingBox = new BoundingBox
                    {
                        X = d.BoundingBox.X,
                        Y = d.BoundingBox.Y,
                        Width = d.BoundingBox.Width,
                        Height = d.BoundingBox.Height
                    }
                };

                db.PlateSightings.Add(sighting);

                // Update summary
                var summary = await db.PlateSummaries.FindAsync(d.Plate);

                if (summary == null)
                {
                    summary = new PlateSummary
                    {
                        Plate = d.Plate,
                        IssueState = "",
                        TotalCount = 1,
                        LastSeen = d.Timestamp
                    };

                    db.PlateSummaries.Add(summary);
                }
                else
                {
                    summary.TotalCount++;
                    summary.LastSeen = d.Timestamp;
                }
            }

            video.ProcessingStatus = "Completed";
            await db.SaveChangesAsync(token);

            _logger.LogInformation("Completed processing video {VideoId}", video.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing video {VideoId}", video.Id);
            video.ProcessingStatus = "Failed";
            await db.SaveChangesAsync(token);
        }
    }
}