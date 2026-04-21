using alpr.api.Helpers;
using alpr.api.Services.Interfaces;
using alpr.api.Services.Models;
using alpr.api.Shared;

namespace alpr.api.Services;

public class AlprEngine : IAlprEngine
{
    public Task<AlprResult> ProcessVideoAsync(string filePath)
    {
        return Task.Run(() =>
        {
            // Allocate buffer for native detections
            var native = new AlprEngineNative.NativePlateDetection[EngineConstants.MAX_DETECTIONS_BUFFER];

            // Call into the native DLL
            int count = AlprEngineNative.ProcessVideo(filePath, native, EngineConstants.MAX_DETECTIONS_BUFFER);

            var result = new AlprResult();

            for (int i = 0; i < count; i++)
            {
                var n = native[i];

                result.Detections.Add(new PlateDetection
                {
                    Plate = n.Plate,
                    Timestamp = DateTime.UnixEpoch.AddSeconds(n.TimestampSeconds),
                    FrameNumber = n.FrameNumber,
                    Confidence = n.Confidence,

                    BoundingBox = new BoundingBox
                    {
                        X = n.BoundingBox.X,
                        Y = n.BoundingBox.Y,
                        Width = n.BoundingBox.Width,
                        Height = n.BoundingBox.Height
                    }
                });
            }

            return result;
        });
    }
}