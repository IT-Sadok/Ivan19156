using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Infrastructure;

namespace IoT.IntegrationTests.Infrastructure;

public static class TestDataSeeder
{
    private static readonly DateTime SeedTime = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Seed(AppDbContext db)
    {
        SeedTechnician(db);
        SeedDevice(db);
        db.SaveChanges();
    }

    private static void SeedTechnician(AppDbContext db)
    {
        if (db.Users.Any(u => u.Id == TestConstants.TechnicianId))
            return;

        db.Users.Add(new User
        {
            Id = TestConstants.TechnicianId,
            UserName = "technician@test.com",
            NormalizedUserName = "TECHNICIAN@TEST.COM",
            Email = "technician@test.com",
            NormalizedEmail = "TECHNICIAN@TEST.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedAt = SeedTime
        });
    }

    private static void SeedDevice(AppDbContext db)
    {
        if (db.Set<Device>().Any(d => d.Id == TestConstants.DeviceId))
            return;

        db.Set<SensorDevice>().Add(new SensorDevice
        {
            Id = TestConstants.DeviceId,
            Name = "Test Sensor",
            Type = DeviceType.Sensor,
            AdminStatus = DeviceAdminStatus.Active,
            SensorType = SensorType.Temperature,
            CreatedAt = SeedTime,
            UpdatedAt = SeedTime
        });
    }
}