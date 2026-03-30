using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Workers;

public class VideoProcessingWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<VideoProcessingWorker> _logger;

    public VideoProcessingWorker(IServiceProvider services, ILogger<VideoProcessingWorker> logger)
    {
        _services = services;
        _logger = logger;
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

        _logger.LogInformation("Processing video {Id}...", video.Id);

        video.ProcessingStatus = VideoProcessingStatus.PROCESSING;
        await db.SaveChangesAsync(token);

        // Simulate processing
        await Task.Delay(2000, token);

        // Fake detection
        var sighting = new PlateSighting
        {
            Plate = "ABC123",
            Timestamp = DateTime.UtcNow,
            VideoId = video.Id,
            FrameNumber = 42,
            Confidence = 0.92
        };

        db.PlateSightings.Add(sighting);

        // Update summary
        var summary = await db.PlateSummaries.FindAsync("ABC123");

        if (summary == null)
        {
            summary = new PlateSummary
            {
                Plate = "ABC123",
                State = "IL",
                TotalCount = 1,
                LastSeen = DateTime.UtcNow
            };

            db.PlateSummaries.Add(summary);
        }
        else
        {
            summary.TotalCount++;
            summary.LastSeen = DateTime.UtcNow;
        }

        video.ProcessingStatus = VideoProcessingStatus.COMPLETED;
        await db.SaveChangesAsync(token);

        _logger.LogInformation("Completed video {Id}.", video.Id);
    }
}