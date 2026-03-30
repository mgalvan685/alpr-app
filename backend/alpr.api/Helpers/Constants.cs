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

