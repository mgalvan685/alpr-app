namespace alpr.api.DTOs;

public record PlateEvent
{
    public string Plate { get; init; } = default!;
    public DateTime Timestamp { get; init; }
    public int VideoId { get; init; }
    public int FrameNumber { get; init; }
    public double Confidence { get; init; }

    public PlateEvent() { }

    public PlateEvent(string plate, DateTime timestamp, int videoId, int frameNumber, double confidence)
    {
        Plate = plate;
        Timestamp = timestamp;
        VideoId = videoId;
        FrameNumber = frameNumber;
        Confidence = confidence;
    }
}