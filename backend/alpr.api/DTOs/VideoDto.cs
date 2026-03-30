namespace alpr.api.DTOs;

/// <summary>
/// The return model for video-related API endpoints
/// </summary>

public record VideoDto
{
    public int Id { get; init; }
    public string FileName { get; init; } = default!;
    public DateTime UploadTime { get; init; }
    public string ProcessingStatus { get; init; } = default!;

    public VideoDto() { }

    public VideoDto(int id, string fileName, DateTime uploadTime, string processingStatus)
    {
        Id = id;
        FileName = fileName;
        UploadTime = uploadTime;
        ProcessingStatus = processingStatus;
    }
}
