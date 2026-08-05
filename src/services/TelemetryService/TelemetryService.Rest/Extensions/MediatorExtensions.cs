using IoT.Shared.Common;
using IoT.Shared.Mediator;
using TelemetryService.Application.Commands.ProcessTelemetry;

namespace TelemetryService.Rest.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<
        IRequestHandler<ProcessTelemetryCommand, Result<bool>>,
            ProcessTelemetryCommandHandler>();
        return services;
    }
}