using IoT.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("public");

        builder.Entity<User>(entity =>
        {
            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("now()");
        });
        
        // TPT для Device
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
    }
}
