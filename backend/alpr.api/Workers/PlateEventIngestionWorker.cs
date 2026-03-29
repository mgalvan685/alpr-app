using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Workers;

public class PlateEventIngestionWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PlateEventIngestionWorker> _logger;

    public PlateEventIngestionWorker(
        IServiceProvider serviceProvider,
        ILogger<PlateEventIngestionWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PlateEventIngestionWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: Replace this with real event queue consumption
            var simulatedEvent = GenerateFakePlateEvent();

            await ProcessPlateEvent(simulatedEvent, stoppingToken);

            await Task.Delay(1000, stoppingToken); // simulate event frequency
        }
    }

    private async Task ProcessPlateEvent(PlateEvent plateEvent, CancellationToken token)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AlprDbContext>();

        // Insert sighting
        var sighting = new PlateSighting
        {
            Plate = plateEvent.Plate,
            Timestamp = plateEvent.Timestamp,
            VideoId = plateEvent.VideoId,
            FrameNumber = plateEvent.FrameNumber,
            Confidence = plateEvent.Confidence
        };

        db.PlateSightings.Add(sighting);

        // Update summary
        var summary = await db.PlateSummaries
            .FirstOrDefaultAsync(p => p.Plate == plateEvent.Plate, token);

        if (summary == null)
        {
            summary = new PlateSummary
            {
                Plate = plateEvent.Plate,
                TotalCount = 1,
                LastSeen = plateEvent.Timestamp
            };
            db.PlateSummaries.Add(summary);
        }
        else
        {
            summary.TotalCount++;
            summary.LastSeen = plateEvent.Timestamp;
        }

        await db.SaveChangesAsync(token);

        _logger.LogInformation("Processed plate event for {Plate}", plateEvent.Plate);
    }

    // TODO: Remove this method once real events are being ingested
    private PlateEvent GenerateFakePlateEvent()
    {
        return new PlateEvent(
            "ABC123",
            DateTime.UtcNow,
            1,
            Random.Shared.Next(1, 5000),
            0.95
        );
    }
}



