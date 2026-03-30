namespace alpr.api.Database.Models;

/// <summary>
/// Represents aggregated stats for each plate, including total sightings and last seen timestamp. This is a denormalized table for 
/// quick access to summary data without needing to compute it on the fly from PlateSightings.
/// 
/// NOTE: Changes will need to be reflected in AlprDbContext.cs
/// </summary

public class PlateSummary
{
    /// <summary>
    /// Primary key
    /// </summary>
    public string Plate { get; set; } = default!;

    /// <summary>
    /// The 2 letter state the plat is from (ex: "IL" for Illinois)
    /// </summary>
    public string State { get; set; } = default!;

    /// <summary>
    /// How many times the plate appeared in all uploaded videos
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Timestamp of the most recent sighting
    /// </summary>
    public DateTime LastSeen { get; set; }
}
