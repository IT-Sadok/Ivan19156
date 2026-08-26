using DeviceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class DeviceDbContextFactory : IDesignTimeDbContextFactory<DeviceDbContext>
{
    public DeviceDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), 
                "../DeviceService.Rest"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("Default")
                               ?? "Host=localhost;Database=iot;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<DeviceDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DeviceDbContext(optionsBuilder.Options);
    }
}