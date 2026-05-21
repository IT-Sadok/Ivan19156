using AutoMapper;
using IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;
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
    private readonly IMapper _mapper;

    public DeviceCommandsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid deviceId)
        => HandleResult(await _mediator.Send(new GetDeviceCommandsQuery(deviceId)));

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid deviceId,
        [FromBody] CreateDeviceCommandRequest request)
    {
        var command = _mapper.Map<CreateDeviceCommandCommand>(request) with { DeviceId = deviceId };
        return HandleResult(await _mediator.Send(command));
    }
}
