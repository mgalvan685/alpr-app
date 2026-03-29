namespace alpr.api.Database.Models;

public class PlateSighting
{
    public int Id { get; set; }
    public string Plate { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public int VideoId { get; set; }
    public int FrameNumber { get; set; }
    public required double Confidence { get; set; }
}
