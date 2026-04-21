using alpr.api.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alpr.api.Database.Models;

public class PlateSighting
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Plate { get; set; } = string.Empty;

    public string? IssueState { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public int VideoId { get; set; }

    [ForeignKey(nameof(VideoId))]
    public Video? Video { get; set; }

    /// <summary>
    /// Frame number returned by the ALPR engine.
    /// Matches extracted frame file: frame_00023.jpg
    /// </summary>
    [Required]
    public int FrameNumber { get; set; }

    /// <summary>
    /// URL served by static file middleware.
    /// Example: /frames/12/frame_00023.jpg
    /// </summary>
    [Required]
    public string FrameUrl { get; set; } = string.Empty;

    /// <summary>
    /// Confidence score from ALPR engine (0–1)
    /// </summary>
    [Required]
    public double Confidence { get; set; }

    /// <summary>
    /// Bounding box for the detected plate.
    /// </summary>
    [Required]
    public BoundingBox BoundingBox { get; set; } = default!;
}