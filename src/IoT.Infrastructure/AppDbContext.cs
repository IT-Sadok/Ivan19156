using IoT.Domain.Abstractions;
using IoT.Domain.Entities;
using IoT.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace IoT.Infrastructure;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Device> Devices { get; set; }
    public DbSet<SensorDevice> SensorDevices { get; set; }
    public DbSet<ActuatorDevice> ActuatorDevices { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<DeviceLocation> DeviceLocations { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<DeviceCommand> DeviceCommands { get; set; }
    public DbSet<CommandType> CommandTypes { get; set; }
    public DbSet<TelemetryRecord> TelemetryRecords { get; set; }
    public DbSet<DeviceApiKey> DeviceApiKeys { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<AlertAcknowledgement> AlertAcknowledgements { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("vector");

        builder.HasDefaultSchema("public");

        builder.Entity<User>(entity =>
        {
            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("now()");
        });
        
        // TPT for Device
        builder.Entity<Device>().ToTable("Devices");
        builder.Entity<SensorDevice>().ToTable("SensorDevices");
        builder.Entity<ActuatorDevice>().ToTable("ActuatorDevices");

        // DeviceLocation — partial unique index
        builder.Entity<DeviceLocation>()
            .HasIndex(dl => dl.DeviceId)
            .HasFilter("\"RemovedAt\" IS NULL")
            .IsUnique();

        // BaseEntity defaults
        builder.Entity<Device>()
            .Property(d => d.CreatedAt)
            .HasDefaultValueSql("now()");

        builder.Entity<Device>()
            .Property(d => d.UpdatedAt)
            .HasDefaultValueSql("now()");
        
        builder.Entity<DeviceCommand>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("now()");

        builder.Entity<DeviceCommand>()
            .Property(c => c.UpdatedAt)
            .HasDefaultValueSql("now()");

        builder.Entity<CommandType>()
            .HasIndex(ct => ct.Slug)
            .IsUnique();
        
        builder.Entity<TelemetryRecord>()
            .HasIndex(t => new { t.DeviceId, t.MessageId })
            .IsUnique();

        builder.Entity<TelemetryRecord>()
            .Property(t => t.ReceivedAt)
            .HasDefaultValueSql("now()");
        
        builder.Entity<Rule>()
            .HasCheckConstraint(
                "chk_rules_device_or_type",
                "(\"DeviceId\" IS NULL) != (\"DeviceType\" IS NULL)");

        builder.Entity<Alert>()
            .HasIndex(a => new { a.DeviceId, a.Status })
            .HasDatabaseName("idx_alerts_device_status");

        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.Entity<CommandType>().HasData(
            new CommandType { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000000"), Slug = "turn_on",          Description = "Turn on the device",       CreatedAt = seedDate, UpdatedAt = seedDate },
            new CommandType { Id = Guid.Parse("a1b2c3d4-0002-0000-0000-000000000000"), Slug = "turn_off",         Description = "Turn off the device",      CreatedAt = seedDate, UpdatedAt = seedDate },
            new CommandType { Id = Guid.Parse("a1b2c3d4-0003-0000-0000-000000000000"), Slug = "reboot",           Description = "Reboot the device",        CreatedAt = seedDate, UpdatedAt = seedDate },
            new CommandType { Id = Guid.Parse("a1b2c3d4-0004-0000-0000-000000000000"), Slug = "set_temperature",  Description = "Set target temperature",   CreatedAt = seedDate, UpdatedAt = seedDate },
            new CommandType { Id = Guid.Parse("a1b2c3d4-0005-0000-0000-000000000000"), Slug = "update_firmware",  Description = "Update device firmware",   CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        builder.Entity<SensorDevice>().HasData(
            new SensorDevice { Id = Guid.Parse("b1c2d3e4-0001-0000-0000-000000000000"), Name = "Test Temperature Sensor", Type = DeviceType.Sensor, AdminStatus = DeviceAdminStatus.Active, SensorType = SensorType.Temperature, MeasurementUnit = "°C", CreatedAt = seedDate, UpdatedAt = seedDate },
            new SensorDevice { Id = Guid.Parse("b1c2d3e4-0002-0000-0000-000000000000"), Name = "Test Humidity Sensor",    Type = DeviceType.Sensor, AdminStatus = DeviceAdminStatus.Active, SensorType = SensorType.Humidity,    MeasurementUnit = "%",  CreatedAt = seedDate, UpdatedAt = seedDate }
        );
        
        builder.Entity<MaintenanceRecord>()
            .Property(m => m.NotesEmbedding)
            .HasColumnType("vector(1536)")
            .HasConversion(
                v => v == null ? null : new Vector(v),
                v => v == null ? null : v.ToArray());
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
