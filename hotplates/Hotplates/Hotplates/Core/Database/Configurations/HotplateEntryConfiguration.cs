using Hotplates.Core.Database.Models;
using Hotplates.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotplates.Core.Database.Configurations;

public class HotplateEntryConfiguration : IEntityTypeConfiguration<HotplateEntry>
{
    public void Configure(EntityTypeBuilder<HotplateEntry> builder)
    {
        builder.ToTable("HotplateEntries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Plate)
            .IsRequired()
            .HasMaxLength(DatabaseConstants.MAX_PLATE_LENGTH);

        builder.HasIndex(x => new { x.Plate, x.State });

        builder.HasMany(x => x.Sightings)
            .WithOne(x => x.HotplateEntry)
            .HasForeignKey(x => x.HotplateEntryId);
    }
}
