namespace alpr.api.Helpers;

/// <summary>
/// Holds constants for the entire project. The parameters should be in SCREAMING_SNAKE_CASE and should be grouped by category 
/// (e.g. VideoProcessingStatus) for better organization and readability.
/// </summary>

public static class VideoProcessingStatus
{
    public const string PENDING = "Pending";
    public const string PROCESSING = "Processing";
    public const string COMPLETED = "Completed";
    public const string FAILED = "Failed";
}

public static class VideoConstants
{
    public static readonly string[] ALLOWED_EXTENSIONS =
    {
        ".mp4", ".mov", ".avi", ".mkv", ".wmv"
    };

    // 500 MB limit for uploaded videos; adjust as needed based on expected use cases and server capabilities
    //public const long MAX_FILE_SIZE = 500 * 1024 * 1024; // 500 MB
    public const long MAX_FILE_SIZE = 600L * 1024L * 1024L; // 600 MB

    // Extract one frame per second of video for ALPR processing; adjust as needed for performance/accuracy tradeoff
    public const int FRAME_EXTRACTION_INTERVAL_MS = 1000;
}

public static class EngineConstants
{
    // Sets the max allowed detections to be returned from the C++ engine
    public const int MAX_DETECTIONS_BUFFER = 1024;
}
