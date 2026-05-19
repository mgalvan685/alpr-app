using Hotplates.Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotplates.Core.Database;

public class HotplatesDbContext : DbContext
{
    public HotplatesDbContext(DbContextOptions<HotplatesDbContext> options)
        : base(options)
    {
    }

    public DbSet<HotplateEntry> HotplateEntries => Set<HotplateEntry>();
    public DbSet<HotplateSighting> HotplateSightings => Set<HotplateSighting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotplatesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
