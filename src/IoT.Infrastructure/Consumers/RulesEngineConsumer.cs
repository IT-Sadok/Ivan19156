using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Domain.Events;
using IoT.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IoT.Infrastructure.Consumers;

public class RulesEngineConsumer : IConsumer<TelemetryReceivedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RulesEngineConsumer> _logger;

    public RulesEngineConsumer(IUnitOfWork unitOfWork, ILogger<RulesEngineConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TelemetryReceivedEvent> context)
    {
        var message = context.Message;
        var ct = context.CancellationToken;
        if (!TryParsePayload(message.Payload, out var payload)) return;
        
        var device = await _unitOfWork.Devices.GetByIdAsync(message.DeviceId, ct);
        if (device == null) return;

        var rules = await _unitOfWork.Rules.GetActiveByDeviceAsync(message.DeviceId, device.Type, ct);
        if (!rules.Any()) return;
        

        var triggeredAlerts = rules
            .Where(rule => EvaluateRule(rule, payload!))
            .Select(rule => new Alert
            {
                Id = Guid.NewGuid(),
                DeviceId = message.DeviceId,
                RuleId = rule.Id,
                Status = AlertStatus.New,
                TriggeredAt = message.ReceivedAt
            })
            .ToList();

        if (!triggeredAlerts.Any()) return;

        _logger.LogInformation(
            "Rules triggered for device {DeviceId}: {Count} alerts",
            message.DeviceId, triggeredAlerts.Count);

        await _unitOfWork.Alerts.AddRangeAsync(triggeredAlerts, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    private bool TryParsePayload(string payload, out JsonDocument? document)
    {
        try
        {
            document = JsonDocument.Parse(payload);
            return true;
        }
        catch
        {
            _logger.LogWarning("Failed to parse telemetry payload");
            document = null;
            return false;
        }
    }

    private static bool EvaluateRule(Rule rule, JsonDocument payload)
    {
        if (!payload.RootElement.TryGetProperty(rule.Field, out var element)
            || !element.TryGetDouble(out var value))
            return false;

        return rule.Operator switch
        {
            RuleOperator.Gt => value > rule.Value,
            RuleOperator.Lt => value < rule.Value,
            RuleOperator.Eq => value == rule.Value,
            RuleOperator.Neq => value != rule.Value,
            RuleOperator.Gte => value >= rule.Value,
            RuleOperator.Lte => value <= rule.Value,
            _ => false
        };
    }
}