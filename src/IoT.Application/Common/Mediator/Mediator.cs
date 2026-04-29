using IoT.Interfaces.Mediator;
using Microsoft.Extensions.DependencyInjection;
namespace IoT.Application.Common.Mediator;
public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
        => _provider = provider;

    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken ct = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        dynamic handler = _provider.GetRequiredService(handlerType);

        var behaviorsType = typeof(IEnumerable<>).MakeGenericType(
            typeof(IPipelineBehavior<,>)
                .MakeGenericType(requestType, typeof(TResponse)));

        dynamic behaviors = _provider.GetRequiredService(behaviorsType);

        RequestHandlerDelegate<TResponse> pipeline = () =>
            handler.Handle((dynamic)request, ct);

        foreach (var behavior in Enumerable.Reverse(behaviors))
        {
            var next = pipeline;
            pipeline = () => behavior.Handle((dynamic)request, next, ct);
        }

        return pipeline();
    }
}