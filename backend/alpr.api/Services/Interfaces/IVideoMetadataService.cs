using alpr.api.Services.Models;

namespace alpr.api.Services.Interfaces;

public interface IVideoMetadataService
{
    Task<VideoMetadata?> ExtractAsync(string filePath);
}