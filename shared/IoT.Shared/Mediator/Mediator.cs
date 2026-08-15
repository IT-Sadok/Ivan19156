using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IoT.Shared.Mediator;

public sealed class Mediator(
    IServiceScopeFactory scopeFactory,
    ILogger<Mediator> logger) : IMediator
{
    public async Task<TResult> SendAsync<TRequest, TResult>(
        TRequest request,
        CancellationToken ct = default)
        where TRequest : IRequest<TResult>
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName}", requestName);
        var sw = Stopwatch.StartNew();

        await using var scope = scopeFactory.CreateAsyncScope();

        var validators = scope.ServiceProvider
            .GetService<IEnumerable<IValidator<TRequest>>>();

        if (validators is not null && validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }

        var handler = scope.ServiceProvider
            .GetRequiredService<IRequestHandler<TRequest, TResult>>();
        var result = await handler.ExecuteAsync(request, ct);

        sw.Stop();
        logger.LogInformation(
            "Handled {RequestName} in {ElapsedMs}ms",
            requestName,
            sw.ElapsedMilliseconds);

        return result;
    }
}