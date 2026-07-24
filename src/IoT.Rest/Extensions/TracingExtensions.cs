using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace IoT.Rest.Extensions;

public static class TracingExtensions
{
    public static WebApplicationBuilder AddTracing(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("iot-api"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(
                            builder.Configuration["Jaeger:Endpoint"] 
                            ?? "http://localhost:4317");
                    });
            });

        return builder;
    }
}