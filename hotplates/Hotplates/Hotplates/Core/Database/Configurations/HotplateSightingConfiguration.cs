using Hotplates.Core.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotplates.Core.Database.Configurations;

public class HotplateSightingConfiguration : IEntityTypeConfiguration<HotplateSighting>
{
    public void Configure(EntityTypeBuilder<HotplateSighting> builder)
    {
        builder.ToTable("HotplateSightings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Plate)
            .IsRequired()
            .HasMaxLength(16);

        builder.HasIndex(x => x.Timestamp);

        builder.HasIndex(x => new { x.Latitude, x.Longitude });

        builder.Property(x => x.RawMetadataJson)
            .HasColumnType("jsonb");
    }
}
