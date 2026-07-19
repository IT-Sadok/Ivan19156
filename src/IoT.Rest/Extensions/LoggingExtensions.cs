using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace IoT.Rest.Extensions;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                    new Uri(context.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200"))
                {
                    IndexFormat = "iot-logs-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 1,
                    NumberOfReplicas = 0,
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog
                });
    
            Serilog.Debugging.SelfLog.Enable(Console.Error);
        });

        return builder;
    }
}