using IoT.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IoT.Rest.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));

        return services;
    }
}