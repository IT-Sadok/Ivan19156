using DeviceService.Application.Commands.CreateRule;
using DeviceService.Application.Queries.GetRules;
using DeviceService.Domain.Enums;
using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Tech debt — DeviceService.Rest depends on IoT.Domain via IoT.Contracts.
// IoT.Contracts uses IoT.Domain.Enums directly (CreateDeviceRequest, UpdateDeviceRequest).
// Fix: move enums to IoT.Contracts independently from IoT.Domain.
using IoTDeviceType = IoT.Domain.Enums.DeviceType;

namespace DeviceService.Rest.Controllers;

[Route("api/rules")]
[Authorize]
public class RulesController : BaseController
{
    private readonly IMediator _mediator;

    public RulesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? deviceId)
    {
        var query = new GetRulesQuery(deviceId);
        return HandleResult(await _mediator.SendAsync<GetRulesQuery, Result<IEnumerable<RuleResponse>>>(query));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRuleRequest request)
    {
        DeviceType? deviceType = request.DeviceType switch
        {
            IoTDeviceType.Sensor => DeviceType.Sensor,
            IoTDeviceType.Actuator => DeviceType.Actuator,
            _ => null
        };

        var command = new CreateRuleCommand(
            request.Name,
            request.Field,
            (RuleOperator)(int)request.Operator,
            request.Value,
            (RuleAction)(int)request.Action,
            request.DeviceId,
            deviceType);
        return HandleResult(await _mediator.SendAsync<CreateRuleCommand, Result<Guid>>(command));
    }
}
