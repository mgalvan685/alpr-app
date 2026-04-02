using alpr.api.Helpers;

namespace alpr.api.Database.Models;

/// <summary>
/// Represents an uploaded video file in the system, including its metadata and processing status
/// 
/// NOTE: Changes will need to be reflected in AlprDbContext.cs
/// </summary>

public class Video
{
    /// <summary>
    /// Primary key identifier for the video record. This is an auto-incrementing integer that uniquely identifies each video in the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Stored file name of the uploaded video. This is the original name of the file as provided by the user during upload. It is used for display 
    /// purposes and may not be unique across different uploads.
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the file system path associated with this instance.
    /// </summary>
    public string FilePath { get; set; } = default!;

    /// <summary>
    /// Time the user uploaded the video. This timestamp is recorded at the moment of upload and is used to track when the video was added to 
    /// the system.
    /// </summary>
    public DateTime UploadTime { get; set; }

    /// <summary>
    /// Processing status of the video. This field indicates the current state of the video in the processing pipeline.
    /// </summary>
    public string ProcessingStatus { get; set; } = VideoProcessingStatus.PENDING;

    public virtual VideoMetadata? Metadata { get; set; }
}
