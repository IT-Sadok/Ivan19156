using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Prometheus;

namespace IoT.Rest.Extensions;

public static class MetricsExtensions
{
    public static WebApplicationBuilder AddMetrics(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("iot-api"))
                    .AddRuntimeInstrumentation();
            });

        return builder;
    }

    public static WebApplication UseMetrics(this WebApplication app)
    {
        app.UseHttpMetrics(); 
        app.MapMetrics();     
        return app;
    }
}