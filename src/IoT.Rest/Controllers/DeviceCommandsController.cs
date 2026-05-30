using IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;
using IoT.Application.Common.Mappings;
using IoT.Application.Queries.DeviceCommands.GetDeviceCommands;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Constants;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Authorize]
[Route("api/devices/{deviceId:guid}/commands")]
public class DeviceCommandsController : BaseController
{
    private readonly IMediator _mediator;

    public DeviceCommandsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid deviceId)
        => HandleResult(await _mediator.Send(new GetDeviceCommandsQuery(deviceId)));

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid deviceId,
        [FromBody] CreateDeviceCommandRequest request)
        => HandleResult(await _mediator.Send(request.ToCommand() with { DeviceId = deviceId }));
}
