// IoT.Api/Extensions/MediatorExtensions.cs
using System.Reflection;
using IoT.Application.Common.Mediator;
using IoT.Interfaces.Mediator;
using IoT.Application.Common.Behaviors;

namespace IoT.Rest.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly applicationAssembly)
    {
        services.AddScoped<IMediator, Mediator>();

       
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

       
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}