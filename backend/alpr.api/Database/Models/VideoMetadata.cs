using alpr.api.Database.Models;

public class VideoMetadata
{
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to Video
    /// </summary>
    public int VideoId { get; set; }

    public double? DurationSeconds { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public double? FrameRate { get; set; }

    /// <summary>
    /// Navigation back to Video for bidirectional relationship
    /// </summary>
    public virtual Video? Video { get; set; }

}