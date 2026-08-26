using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MaintenanceService.Infrastructure.Persistence;

public class MaintenanceDbContextFactory : IDesignTimeDbContextFactory<MaintenanceDbContext>
{
    public MaintenanceDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MaintenanceService.Rest"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("Default")
                               ?? "Host=localhost;Database=iot;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<MaintenanceDbContext>();
        optionsBuilder.UseNpgsql(connectionString, o => o.UseVector());

        return new MaintenanceDbContext(optionsBuilder.Options);
    }
}