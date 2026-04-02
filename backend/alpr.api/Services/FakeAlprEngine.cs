using alpr.api.Services.Interfaces;
using alpr.api.Services.Models;

namespace alpr.api.Services;

public class FakeAlprEngine : IAlprEngine
{
    public Task<AlprResult> ProcessVideoAsync(string filePath)
    {
        // Simulate processing delay
        Thread.Sleep(1000);

        var result = new AlprResult
        {
            Detections =
            {
                new PlateDetection
                {
                    Plate = "ABC123",
                    Timestamp = DateTime.UtcNow,
                    FrameNumber = 42,
                    Confidence = 0.92
                }
            }
        };

        return Task.FromResult(result);
    }
}