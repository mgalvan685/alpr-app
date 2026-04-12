using alpr.api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace alpr.api.Database;

public class AlprDbContext : DbContext
{
    public AlprDbContext(DbContextOptions<AlprDbContext> options)
        : base(options)
    {
    }

    public DbSet<Video> Videos => Set<Video>();
    public DbSet<VideoMetadata> VideoMetadata => Set<VideoMetadata>();
    public DbSet<PlateSighting> PlateSightings => Set<PlateSighting>();
    public DbSet<PlateSummary> PlateSummaries => Set<PlateSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Video table
        modelBuilder.Entity<Video>()
            .ToTable("videos")
            .HasOne(v => v.Metadata)
            .WithOne(m => m.Video)
            .HasForeignKey<VideoMetadata>(m => m.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VideoMetadata>()
            .ToTable("video_metadata");

        // PlateSightings table
        modelBuilder.Entity<PlateSighting>(entity =>
        {
            entity.ToTable("plate_sightings");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Plate).IsRequired();
            entity.Property(p => p.IssueState);
            entity.Property(p => p.Timestamp).IsRequired();
            entity.Property(p => p.FrameNumber).IsRequired();
            entity.Property(p => p.Confidence).IsRequired();

            entity.HasOne<Video>()
                  .WithMany()
                  .HasForeignKey(p => p.VideoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PlateSummaries table
        modelBuilder.Entity<PlateSummary>(entity =>
        {
            entity.ToTable("plate_summaries");
            entity.HasKey(p => p.Plate);

            entity.Property(p => p.IssueState);
            entity.Property(p => p.TotalCount).IsRequired();
            entity.Property(p => p.LastSeen).IsRequired();
        });
    }
}