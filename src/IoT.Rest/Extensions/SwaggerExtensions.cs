// IoT.Api/Extensions/SwaggerExtensions.cs
namespace IoT.Rest.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}