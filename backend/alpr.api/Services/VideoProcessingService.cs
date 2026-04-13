using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.Helpers;
using alpr.api.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace alpr.api.Services;

public class VideoProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<VideoProcessingService> _logger;

    public VideoProcessingService(IServiceProvider serviceProvider, ILogger<VideoProcessingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopWatch = new Stopwatch();

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AlprDbContext>();

            // Find next pending video
            var video = await db.Videos
                .Where(v => v.ProcessingStatus == "Pending")
                .OrderBy(v => v.UploadTime)
                .FirstOrDefaultAsync(stoppingToken);

            if (video != null)
            {
                try
                {
                    video.ProcessingStatus = "Processing";
                    await db.SaveChangesAsync(stoppingToken);

                    // Extract frames
                    var tempFolder = Path.Combine("temp_frames", $"video_{video.Id}");

                    stopWatch.StartTimer();
                    var frames = await FrameExtractor.ExtractFramesAsync(video.FilePath, tempFolder, 500);
                    _logger.LogInformation("Extracted {FrameCount} frames from video {VideoId} in {ElapsedMilliseconds} ms", frames.Count, video.Id, stopWatch.StopAndGetElapsed());

                    // Run ALPR on each frame
                    foreach (var frame in frames)
                    {
                        _logger.LogInformation("Processing frame {Frame} for video {VideoId}", frame, video.Id);

                        var plate = new StringBuilder(32);
                        var state = new StringBuilder(16);
                        float confidence;

                        stopWatch.StartTimer();
                        int found = AlprEngine.ProcessFrame(frame, plate, state, out confidence);
                        _logger.LogInformation("Processed frame {Frame} for video {VideoId} in {ElapsedMilliseconds} - Found: {Found}, Plate: {Plate}, State: {State}, Confidence: {Confidence}", frame, video.Id, stopWatch.StopAndGetElapsed(), found, plate.ToString(), state.ToString(), confidence);

                        if (found == 1)
                        {
                            _logger.LogInformation(
                                "Detection: Plate={Plate}, State={State}, Confidence={Confidence}",
                                plate, state, confidence
                            );

                            var sighting = new PlateSighting
                            {
                                Plate = plate.ToString(),
                                IssueState = state.ToString(),
                                Confidence = confidence,
                                Timestamp = ExtractTimestampFromFrameName(frame, video.UploadTime),
                                VideoId = video.Id
                            };

                            db.PlateSightings.Add(sighting);
                            await db.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation(
                                "Saved sighting for plate {Plate} at timestamp {Timestamp}",
                                sighting.Plate,
                                sighting.Timestamp
                            );
                        }
                        else
                        {
                            _logger.LogInformation("No plate detected in frame {FramePath}", frame);
                        }
                    }

                    video.ProcessingStatus = "Completed";
                    await db.SaveChangesAsync(stoppingToken);

                    // Cleanup
                    Directory.Delete(tempFolder, true);
                }
                catch (Exception ex)
                {
                    video.ProcessingStatus = "Failed";
                    Console.WriteLine(ex.ToString());
                    await db.SaveChangesAsync(stoppingToken);
                }
            }

            // Sleep before checking again
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private DateTime ExtractTimestampFromFrameName(string framePath, DateTime uploadTime)
    {
        var file = Path.GetFileNameWithoutExtension(framePath); // frame_00023
        var index = int.Parse(file.Split('_')[1]);

        var ms = index * 500; // if interval = 500ms

        return uploadTime.AddMilliseconds(ms);
    }
}