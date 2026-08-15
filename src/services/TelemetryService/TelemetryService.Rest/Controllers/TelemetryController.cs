using IoT.Contracts.Telemetry;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Mvc;
using TelemetryService.Application.Commands.ProcessTelemetry;
using TelemetryService.Rest.Infrastructure;

namespace TelemetryService.Rest.Controllers;

[Route("api/telemetry")]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
public class TelemetryController : BaseController
{
    private readonly IMediator _mediator;

    public TelemetryController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Process([FromBody] ProcessTelemetryRequest request)
    {
        var deviceId = (Guid)HttpContext.Items["DeviceId"]!;
        var command = new ProcessTelemetryCommand(deviceId, request.MessageId, request.Payload);
        return HandleResult(await _mediator.SendAsync<ProcessTelemetryCommand, Result<bool>>(command));
    }
}