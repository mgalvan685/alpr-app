namespace alpr.api.Database.Models;

public class PlateSummary
{
    public string Plate { get; set; } = default!;
    public int TotalCount { get; set; }
    public DateTime LastSeen { get; set; }
}
