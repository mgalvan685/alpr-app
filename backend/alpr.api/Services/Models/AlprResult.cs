namespace alpr.api.Services.Models;

public class AlprResult
{
    public List<PlateDetection> Detections { get; set; } = new();
}

public class PlateDetection
{
    public string Plate { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int FrameNumber { get; set; }
    public double Confidence { get; set; }
}