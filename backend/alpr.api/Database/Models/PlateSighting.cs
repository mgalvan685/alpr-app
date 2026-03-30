namespace alpr.api.Database.Models;

/// <summary>
/// Represents a single ALPR detection event, capturing the license plate information, timestamp, associated video and frame details, 
/// and confidence level of the detection
/// 
/// NOTE: Changes will need to be reflected in AlprDbContext.cs
/// </summary>
public class PlateSighting
{
    /// <summary>
    /// Unique ID for the sighting
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Text of the detected license plate. This is the primary data captured by the ALPR system and is used for all subsequent processing
    /// </summary>
    public string Plate { get; set; } = default!;

    /// <summary>
    /// Timestamp of when the plate was sighted. This is recorded at the moment the ALPR system detects a plate in a video frame
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Foreign key to the Video object that this sighting belongs to
    /// </summary>
    public int VideoId { get; set; }

    /// <summary>
    /// Frame number where the plate was captured. This helps with debugging and overlays
    /// </summary>
    public int FrameNumber { get; set; }

    /// <summary>
    /// Gets or sets the confidence score representing the certainty of a result
    /// </summary>
    public required double Confidence { get; set; }
}
