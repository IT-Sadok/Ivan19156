using MaintenanceService.Domain.Abstractions;
using MaintenanceService.Domain.Entities;
using MaintenanceService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace MaintenanceService.Infrastructure.Persistence;

public class MaintenanceDbContext : DbContext
{
    public MaintenanceDbContext(DbContextOptions<MaintenanceDbContext> options) : base(options) { }

    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("maintenance");
        

        builder.Entity<MaintenanceRecord>(e =>
        {
            e.ToTable("MaintenanceRecords");
            e.HasKey(r => r.Id);
            e.Property(r => r.Notes).HasMaxLength(4096);
            e.Property(r => r.PerformedAt).IsRequired();
            e.Property(r => r.CreatedAt).HasDefaultValueSql("now()");
            e.Property(r => r.UpdatedAt).HasDefaultValueSql("now()");

            e.Property(r => r.DeviceId)
                .IsRequired()
                .HasConversion(
                    v => v.Value,
                    v => DeviceId.From(v));

            e.Property(r => r.TechnicianId)
                .IsRequired()
                .HasConversion(
                    v => v.Value,
                    v => TechnicianId.From(v));

            e.Property(r => r.NotesEmbedding)
                .HasColumnType("vector(1536)")
                .HasConversion(
                    v => v == null ? null : new Vector(v),
                    v => v == null ? null : v.ToArray())
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<float[]?>(
                    (a, b) => a == null && b == null || (a != null && b != null && a.SequenceEqual(b)),
                    v => v == null ? 0 : v.Aggregate(0, (a, e) => HashCode.Combine(a, e.GetHashCode())),
                    v => v == null ? null : v.ToArray()));

            e.HasIndex(r => r.DeviceId);
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