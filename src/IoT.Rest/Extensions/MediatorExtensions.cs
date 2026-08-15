using System.Reflection;
using FluentValidation;
using IoT.Shared.Mediator;
using IoT.Contracts.Devices;

namespace IoT.Rest.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly applicationAssembly)
    {
        services.AddSingleton<IMediator, Mediator>();

        var handlerTypes = applicationAssembly
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

        foreach (var handlerType in handlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            services.AddScoped(interfaceType, handlerType);
        }

        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddValidatorsFromAssembly(typeof(DeviceResponse).Assembly);

        return services;
    }
}