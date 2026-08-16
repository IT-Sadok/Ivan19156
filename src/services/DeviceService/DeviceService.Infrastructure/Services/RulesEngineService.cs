using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Interfaces;
using DeviceService.Interfaces.Services;

namespace DeviceService.Infrastructure.Services;

public class RulesEngineService : IRulesEngineService
{
    private readonly IUnitOfWork _unitOfWork;

    public RulesEngineService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task EvaluateAsync(
        Guid deviceId,
        string metricKey,
        double value,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(deviceId, ct);
        if (device is null)
            return;

        var rules = await _unitOfWork.Rules.GetActiveByDeviceAsync(deviceId, device.Type, ct);

        var matchingRules = rules.Where(r => r.MetricKey == metricKey);

        foreach (var rule in matchingRules)
        {
            if (!IsBreached(rule, value))
                continue;

            var alert = new Alert
            {
                DeviceId = deviceId,
                RuleId = rule.Id,
                Message = $"Rule '{rule.Name}' breached: {metricKey}={value}",
                Status = AlertStatus.New,
                TriggeredValue = value
            };

            await _unitOfWork.Alerts.AddAsync(alert, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }

    private static bool IsBreached(Rule rule, double value) => rule.Operator switch
    {
        RuleOperator.Gt  => value > rule.Threshold,
        RuleOperator.Lt  => value < rule.Threshold,
        RuleOperator.Eq  => Math.Abs(value - rule.Threshold) < 1e-9,
        RuleOperator.Neq => Math.Abs(value - rule.Threshold) >= 1e-9,
        RuleOperator.Gte => value >= rule.Threshold,
        RuleOperator.Lte => value <= rule.Threshold,
        _ => false
    };
}
