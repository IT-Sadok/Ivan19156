namespace IoT.Shared.Mediator;

public interface IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    Task<TResult> ExecuteAsync(TRequest request, CancellationToken ct = default);
}