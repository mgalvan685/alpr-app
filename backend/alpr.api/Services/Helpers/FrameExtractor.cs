using System.Diagnostics;

namespace alpr.api.Services.Helpers;

public static class FrameExtractor
{
    public static async Task<List<string>> ExtractFramesAsync(string videoPath, string outputFolder, int intervalMs)
    {
        try
        {
            Directory.CreateDirectory(outputFolder);

            // Example: frame_%05d.jpg
            string outputPattern = Path.Combine(outputFolder, "frame_%05d.jpg");

            var args = $"-i \"{videoPath}\" -vf fps=1/{intervalMs / 1000.0} \"{outputPattern}\"";

            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = startInfo;

            process.OutputDataReceived += (s, e) => { 
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine($"FFmpeg: {e.Data}");
                }
            };
            process.ErrorDataReceived += (s, e) => { 
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine($"FFmpeg Error: {e.Data}");
                }
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();


            // Collect all extracted frames
            var frames = Directory.GetFiles(outputFolder, "frame_*.jpg")
                                  .OrderBy(f => f)
                                  .ToList();

            return frames;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting frames: {ex.Message}");
            return new List<string>(); // Return empty list on failure
            throw;
        }
    }
}