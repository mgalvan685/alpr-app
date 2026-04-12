using alpr.api.Database;
using alpr.api.Database.Models;
using alpr.api.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Services;

public class VideoProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public VideoProcessingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
                    var frames = await FrameExtractor.ExtractFramesAsync(video.FilePath, tempFolder, 500);

                    // Test the C++ ALPR engine
                    foreach (var frame in frames)
                    {
                        var result = AlprEngine.Analyze(frame);

                        Console.WriteLine(
                            $"Frame: {Path.GetFileName(frame)} → Found={result.found}, Plate={result.plate}, State={result.state}, Conf={result.confidence}"
                        );
                    }

                    // Run ALPR on each frame
                    foreach (var frame in frames)
                    {
                        var result = AlprEngine.Analyze(frame);

                        if (result.found)
                        {
                            var sighting = new PlateSighting
                            {
                                Plate = result.plate,
                                IssueState = result.state,
                                Confidence = result.confidence,
                                Timestamp = ExtractTimestampFromFrameName(frame, video.UploadTime),
                                VideoId = video.Id
                            };

                            db.PlateSightings.Add(sighting);
                            await db.SaveChangesAsync(stoppingToken);
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