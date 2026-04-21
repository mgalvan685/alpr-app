using alpr.api.Shared;

namespace alpr.api.DTOs;

public record PlateDto
{
    public int Id { get; init; }
    public string Plate { get; init; } = string.Empty;
    public string? IssueState { get; init; }
    public BoundingBox? BoundingBox { get; init; }
}