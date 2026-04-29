// IoT.Interfaces/IMediator.cs
using IoT.Interfaces.Mediator;

namespace IoT.Interfaces.Mediator;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}