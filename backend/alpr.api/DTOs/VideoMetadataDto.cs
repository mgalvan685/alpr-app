namespace alpr.api.DTOs;

public record VideoMetadataDto
{
    public int VideoId { get; set; }
    public double? DurationSeconds { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public double? FrameRate { get; set; }

    public VideoMetadataDto(VideoMetadata metadata)
    {
        VideoId = metadata.VideoId;
        DurationSeconds = metadata.DurationSeconds;
        Width = metadata.Width;
        Height = metadata.Height;
        FrameRate = metadata.FrameRate;
    }

}
