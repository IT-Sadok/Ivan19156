using DeviceService.Domain.Abstractions;
using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Persistence;

public class DeviceDbContext : DbContext
{
    public DeviceDbContext(DbContextOptions<DeviceDbContext> options) : base(options) { }

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<SensorDevice> SensorDevices => Set<SensorDevice>();
    public DbSet<ActuatorDevice> ActuatorDevices => Set<ActuatorDevice>();
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<DeviceLocation> DeviceLocations => Set<DeviceLocation>();
    public DbSet<DeviceApiKey> DeviceApiKeys => Set<DeviceApiKey>();
    public DbSet<CommandType> CommandTypes => Set<CommandType>();
    public DbSet<DeviceCommand> DeviceCommands => Set<DeviceCommand>();
    public DbSet<Rule> Rules => Set<Rule>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<AlertAcknowledgement> AlertAcknowledgements => Set<AlertAcknowledgement>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("devices");

        // ── Device (TPT base) ────────────────────────────────────────────────
        builder.Entity<Device>(e =>
        {
            e.ToTable("Devices");
            e.HasKey(d => d.Id);
            e.Property(d => d.Name).IsRequired().HasMaxLength(256);
            e.Property(d => d.Type).IsRequired();
            e.Property(d => d.AdminStatus).IsRequired();
            e.Property(d => d.CreatedAt).HasDefaultValueSql("now()");
            e.Property(d => d.UpdatedAt).HasDefaultValueSql("now()");

            e.HasOne(d => d.Manufacturer)
                .WithMany(m => m.Devices)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── SensorDevice (TPT child) ─────────────────────────────────────────
        builder.Entity<SensorDevice>(e =>
        {
            e.ToTable("SensorDevices");
            e.Property(s => s.SensorType).IsRequired();
            e.Property(s => s.MeasurementUnit).HasMaxLength(64);
        });

        // ── ActuatorDevice (TPT child) ───────────────────────────────────────
        builder.Entity<ActuatorDevice>(e =>
        {
            e.ToTable("ActuatorDevices");
            e.Property(a => a.ActuatorType).IsRequired();
            e.Property(a => a.PowerState).IsRequired();
        });

        // ── Manufacturer ─────────────────────────────────────────────────────
        builder.Entity<Manufacturer>(e =>
        {
            e.ToTable("Manufacturers");
            e.HasKey(m => m.Id);
            e.Property(m => m.Name).IsRequired().HasMaxLength(256);
            e.Property(m => m.CreatedAt).HasDefaultValueSql("now()");
            e.Property(m => m.UpdatedAt).HasDefaultValueSql("now()");
        });

        // ── Warehouse ────────────────────────────────────────────────────────
        builder.Entity<Warehouse>(e =>
        {
            e.ToTable("Warehouses");
            e.HasKey(w => w.Id);
            e.Property(w => w.Name).IsRequired().HasMaxLength(256);
            e.Property(w => w.Address).HasMaxLength(512);
            e.Property(w => w.CreatedAt).HasDefaultValueSql("now()");
            e.Property(w => w.UpdatedAt).HasDefaultValueSql("now()");
        });

        // ── DeviceLocation ───────────────────────────────────────────────────
        builder.Entity<DeviceLocation>(e =>
        {
            e.ToTable("DeviceLocations");
            e.HasKey(dl => dl.Id);
            e.Property(dl => dl.AssignedAt).IsRequired();
            e.Property(dl => dl.CreatedAt).HasDefaultValueSql("now()");
            e.Property(dl => dl.UpdatedAt).HasDefaultValueSql("now()");

            e.HasOne(dl => dl.Device)
                .WithMany(d => d.Locations)
                .HasForeignKey(dl => dl.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(dl => dl.Warehouse)
                .WithMany(w => w.DeviceLocations)
                .HasForeignKey(dl => dl.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Partial unique index: only one active location per device
            e.HasIndex(dl => dl.DeviceId)
                .IsUnique()
                .HasFilter("\"RemovedAt\" IS NULL");
        });

        // ── DeviceApiKey ─────────────────────────────────────────────────────
        builder.Entity<DeviceApiKey>(e =>
        {
            e.ToTable("DeviceApiKeys");
            e.HasKey(k => k.Id);
            e.Property(k => k.KeyHash).IsRequired().HasMaxLength(512);
            e.Property(k => k.Prefix).IsRequired().HasMaxLength(64);
            e.Property(k => k.IsActive).IsRequired();
            e.Property(k => k.CreatedAt).HasDefaultValueSql("now()");
            e.Property(k => k.UpdatedAt).HasDefaultValueSql("now()");

            e.HasOne(k => k.Device)
                .WithMany(d => d.ApiKeys)
                .HasForeignKey(k => k.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(k => k.Prefix).IsUnique();
        });

        // ── CommandType ──────────────────────────────────────────────────────
        builder.Entity<CommandType>(e =>
        {
            e.ToTable("CommandTypes");
            e.HasKey(ct => ct.Id);
            e.Property(ct => ct.Slug).IsRequired().HasMaxLength(128);
            e.Property(ct => ct.Description).HasMaxLength(512);
            e.Property(ct => ct.CreatedAt).HasDefaultValueSql("now()");
            e.Property(ct => ct.UpdatedAt).HasDefaultValueSql("now()");

            e.HasIndex(ct => ct.Slug).IsUnique();

            // Seed data
            e.HasData(
                new CommandType
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    Slug = "turn_on",
                    Description = "Turn the device on",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new CommandType
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000002"),
                    Slug = "turn_off",
                    Description = "Turn the device off",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new CommandType
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000003"),
                    Slug = "reboot",
                    Description = "Reboot the device",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new CommandType
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000004"),
                    Slug = "set_temperature",
                    Description = "Set the target temperature",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new CommandType
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000005"),
                    Slug = "update_firmware",
                    Description = "Update the device firmware",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        });

        // ── DeviceCommand ────────────────────────────────────────────────────
        builder.Entity<DeviceCommand>(e =>
        {
            e.ToTable("DeviceCommands");
            e.HasKey(dc => dc.Id);
            e.Property(dc => dc.Status).IsRequired();
            e.Property(dc => dc.Payload).HasMaxLength(4096);
            e.Property(dc => dc.CreatedAt).HasDefaultValueSql("now()");
            e.Property(dc => dc.UpdatedAt).HasDefaultValueSql("now()");

            e.HasOne(dc => dc.Device)
                .WithMany(d => d.Commands)
                .HasForeignKey(dc => dc.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(dc => dc.CommandType)
                .WithMany()
                .HasForeignKey(dc => dc.CommandTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Rule ─────────────────────────────────────────────────────────────
        builder.Entity<Rule>(e =>
        {
            e.ToTable("Rules");
            e.HasKey(r => r.Id);
            e.Property(r => r.Name).IsRequired().HasMaxLength(256);
            e.Property(r => r.MetricKey).IsRequired().HasMaxLength(128);
            e.Property(r => r.Operator).IsRequired();
            e.Property(r => r.Threshold).IsRequired();
            e.Property(r => r.Action).IsRequired();
            e.Property(r => r.IsActive).IsRequired();
            e.Property(r => r.CreatedAt).HasDefaultValueSql("now()");
            e.Property(r => r.UpdatedAt).HasDefaultValueSql("now()");

            e.HasCheckConstraint(
                "CK_Rule_DeviceOrType",
                "(\"DeviceId\" IS NULL) != (\"DeviceType\" IS NULL)");
        });

        // ── Alert ────────────────────────────────────────────────────────────
        builder.Entity<Alert>(e =>
        {
            e.ToTable("Alerts");
            e.HasKey(a => a.Id);
            e.Property(a => a.Message).IsRequired().HasMaxLength(1024);
            e.Property(a => a.Status).IsRequired();
            e.Property(a => a.TriggeredValue).IsRequired();
            e.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
            e.Property(a => a.UpdatedAt).HasDefaultValueSql("now()");

            e.HasOne(a => a.Device)
                .WithMany(d => d.Alerts)
                .HasForeignKey(a => a.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(a => a.Rule)
                .WithMany()
                .HasForeignKey(a => a.RuleId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(a => new { a.DeviceId, a.Status });
        });

        // ── AlertAcknowledgement ─────────────────────────────────────────────
        builder.Entity<AlertAcknowledgement>(e =>
        {
            e.ToTable("AlertAcknowledgements");
            e.HasKey(aa => aa.Id);
            e.Property(aa => aa.AcknowledgedAt).IsRequired();
            e.Property(aa => aa.Note).HasMaxLength(1024);
            e.Property(aa => aa.CreatedAt).HasDefaultValueSql("now()");
            e.Property(aa => aa.UpdatedAt).HasDefaultValueSql("now()");

            e.HasOne(aa => aa.Alert)
                .WithMany(a => a.Acknowledgements)
                .HasForeignKey(aa => aa.AlertId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        

        builder.Entity<SensorDevice>().HasData(
            new SensorDevice
            {
                Id = new Guid("20000000-0000-0000-0000-000000000001"),
                Name = "Temperature Sensor Alpha",
                Type = DeviceType.Sensor,
                AdminStatus = DeviceAdminStatus.Active,
                SensorType = SensorType.Temperature,
                MeasurementUnit = "°C",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new SensorDevice
            {
                Id = new Guid("20000000-0000-0000-0000-000000000002"),
                Name = "Humidity Sensor Beta",
                Type = DeviceType.Sensor,
                AdminStatus = DeviceAdminStatus.Active,
                SensorType = SensorType.Humidity,
                MeasurementUnit = "%",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
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
