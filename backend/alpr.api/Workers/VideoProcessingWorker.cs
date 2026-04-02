using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.Helpers;
using alpr.api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Workers;

public class VideoProcessingWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<VideoProcessingWorker> _logger;
    private readonly IAlprEngine _engine;

    public VideoProcessingWorker(IServiceProvider services, ILogger<VideoProcessingWorker> logger, IAlprEngine engine)
    {
        _services = services;
        _logger = logger;
        _engine = engine;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessPendingVideos(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessPendingVideos(CancellationToken token)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AlprDbContext>();

        var video = await db.Videos
            .Where(v => v.ProcessingStatus == VideoProcessingStatus.PENDING)
            .OrderBy(v => v.UploadTime)
            .FirstOrDefaultAsync(token);

        if (video == null)
            return;

        _logger.LogInformation("Processing file at path: {Path}", video.FilePath);

        // TODO: Call native ALPR engine with video.FilePath

        video.ProcessingStatus = VideoProcessingStatus.PROCESSING;
        await db.SaveChangesAsync(token);

        // Simulate processing
        await Task.Delay(2000, token);

        var result = await _engine.ProcessVideoAsync(video.FilePath);

        foreach (var d in result.Detections)
        {
            var sighting = new PlateSighting
            {
                Plate = d.Plate,
                Timestamp = d.Timestamp,
                VideoId = video.Id,
                FrameNumber = d.FrameNumber,
                Confidence = d.Confidence
            };

            db.PlateSightings.Add(sighting);

            var summary = await db.PlateSummaries.FindAsync(d.Plate);

            if (summary == null)
            {
                summary = new PlateSummary
                {
                    Plate = d.Plate,
                    State = "IL",
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

        video.ProcessingStatus = VideoProcessingStatus.COMPLETED;
        await db.SaveChangesAsync(token);

        _logger.LogInformation("Completed video {Id}.", video.Id);
    }
}