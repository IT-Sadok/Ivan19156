namespace IoT.Shared.Mediator;

public interface IMediator
{
    Task<TResult> SendAsync<TRequest, TResult>(
        TRequest request,
        CancellationToken ct = default)
        where TRequest : IRequest<TResult>;
}