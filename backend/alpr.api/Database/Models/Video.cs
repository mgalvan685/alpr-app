namespace alpr.api.Database.Models;

public class Video
{
    public int Id { get; set; }
    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public DateTime UploadTime { get; set; }
    public string ProcessingStatus { get; set; } = "Pending";

}
