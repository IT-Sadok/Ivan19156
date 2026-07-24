using Prometheus;

namespace IoT.Rest.Extensions;

public static class MetricsExtensions
{
    public static WebApplicationBuilder AddMetrics(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplication UseMetrics(this WebApplication app)
    {
        app.UseHttpMetrics();
        app.MapMetrics();
        return app;
    }
}