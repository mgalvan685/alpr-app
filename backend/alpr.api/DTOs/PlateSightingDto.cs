namespace alpr.api.DTOs;

public record PlateSightingDto
{
    public int Id { get; init; }
    public string Plate { get; init; } = default!;
    public string IssueState { get; init; } = default!;
    public DateTime Timestamp { get; init; }
    public int VideoId { get; init; }
    public int FrameNumber { get; init; }
    public double Confidence { get; init; }

    public PlateSightingDto() { }

    public PlateSightingDto(int id, string plate, string issueState, DateTime timestamp, int videoId, int frameNumber, double confidence)
    {
        Id = id;
        Plate = plate;
        IssueState = issueState;
        Timestamp = timestamp;
        VideoId = videoId;
        FrameNumber = frameNumber;
        Confidence = confidence;
    }
}
