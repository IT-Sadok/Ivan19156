using DeviceService.Interfaces;
using DeviceService.Interfaces.Services;
using IoT.Contracts.Telemetry;
using MassTransit;

namespace DeviceService.Infrastructure.Consumers;

public class TelemetryStoredEventConsumer : IConsumer<TelemetryStoredEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRulesEngineService _rulesEngineService;

    public TelemetryStoredEventConsumer(
        IUnitOfWork unitOfWork,
        IRulesEngineService rulesEngineService)
    {
        _unitOfWork = unitOfWork;
        _rulesEngineService = rulesEngineService;
    }

    public async Task Consume(ConsumeContext<TelemetryStoredEvent> context)
    {
        var message = context.Message;
        var ct = context.CancellationToken;

        await _rulesEngineService.EvaluateAsync(message.DeviceId, "heartbeat", 1.0, ct);
    }
}
