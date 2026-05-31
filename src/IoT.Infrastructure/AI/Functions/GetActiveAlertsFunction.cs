using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.AI.Functions;

public class GetActiveAlertsFunction(IAlertRepository alertRepository) : AIFunctionBase
{
    public override string Name => "get_active_alerts";
    public override string Description => "Returns all currently active alerts in the system";
    public override BinaryData Parameters => BinaryData.FromString("""
                                                                   {
                                                                       "type": "object",
                                                                       "properties": {},
                                                                       "required": []
                                                                   }
                                                                   """);

    public override async Task<string> ExecuteAsync(string arguments, CancellationToken ct = default)
    {
        var alerts = await alertRepository.GetActiveAsync(ct);
        var result = alerts.Select(a => new
        {
            id = a.Id,
            deviceName = a.Device.Name,
            ruleName = a.Rule.Name,
            triggeredAt = a.TriggeredAt,
            status = a.Status.ToString()
        });
        return SerializeResult(result);
    }
}