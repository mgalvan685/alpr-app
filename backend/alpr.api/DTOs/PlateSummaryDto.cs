namespace alpr.api.DTOs;

public record PlateSummaryDto
{
    public string Plate { get; init; } = default!;
    public string State { get; init; } = default!;
    public int TotalCount { get; init; }
    public DateTime LastSeen { get; init; }

    public PlateSummaryDto() { }

    public PlateSummaryDto(string plate, string state, int totalCount, DateTime lastSeen)
    {
        Plate = plate;
        State = state;
        TotalCount = totalCount;
        LastSeen = lastSeen;
    }
}
