using Microsoft.EntityFrameworkCore;
using TelemetryService.Domain.Abstractions;
using TelemetryService.Domain.Entities;
using TelemetryService.Domain.ValueObjects;

namespace TelemetryService.Infrastructure.Persistence;

public class TelemetryDbContext : DbContext
{
    public TelemetryDbContext(DbContextOptions<TelemetryDbContext> options)
        : base(options) { }

    public DbSet<TelemetryRecord> TelemetryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("telemetry");

        builder.Entity<TelemetryRecord>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.DeviceId)
                .HasConversion(
                    v => v.Value,
                    v => DeviceId.From(v));

            entity.HasIndex(t => new { t.DeviceId, t.MessageId })
                .IsUnique();

            entity.Property(t => t.ReceivedAt)
                .HasDefaultValueSql("now()");
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return await base.SaveChangesAsync(ct);
    }
}