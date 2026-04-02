using alpr.api.Services.Interfaces;
using alpr.api.Services.Models;
using System.Diagnostics;
using System.Text.Json;

namespace alpr.api.Services;

public class FfprobeMetadataService : IVideoMetadataService
{
    public async Task<VideoMetadata?> ExtractAsync(string filePath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffprobe",
            Arguments = $"-v quiet -print_format json -show_streams \"{filePath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process == null)
            return null;

        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        using var doc = JsonDocument.Parse(output);
        var streams = doc.RootElement.GetProperty("streams");

        // Find the video stream
        var videoStream = streams.EnumerateArray()
            .FirstOrDefault(s => s.GetProperty("codec_type").GetString() == "video");

        if (videoStream.ValueKind == JsonValueKind.Undefined)
            return null;

        var width = videoStream.GetProperty("width").GetInt32();
        var height = videoStream.GetProperty("height").GetInt32();

        var duration = videoStream.TryGetProperty("duration", out var durProp)
            ? double.Parse(durProp.GetString()!)
            : 0;

        var frameRateStr = videoStream.GetProperty("r_frame_rate").GetString()!;
        var frameRate = ParseFrameRate(frameRateStr);

        return new VideoMetadata
        {
            DurationSeconds = duration,
            Width = width,
            Height = height,
            FrameRate = frameRate
        };
    }

    private double ParseFrameRate(string fr)
    {
        // Example: "30000/1001"
        if (fr.Contains('/'))
        {
            var parts = fr.Split('/');
            if (double.TryParse(parts[0], out var num) &&
                double.TryParse(parts[1], out var den))
            {
                return num / den;
            }
        }

        // Example: "30"
        if (double.TryParse(fr, out var simple))
            return simple;

        return 0;
    }
}