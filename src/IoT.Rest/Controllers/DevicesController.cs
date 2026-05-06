using IoT.Application.Devices.Commands.CreateDevice;
using IoT.Application.Devices.Commands.DeleteDevice;
using IoT.Application.Devices.Commands.UpdateDevice;
using IoT.Application.Devices.Queries.GetDeviceById;
using IoT.Application.Devices.Queries.GetDevices;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/devices")]
public class DevicesController : BaseController
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => HandleResult(await _mediator.Send(new GetDevicesQuery()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => HandleResult(await _mediator.Send(new GetDeviceByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeviceCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleCreatedResult(
            result,
            nameof(GetById),
            new { id = result.Value?.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        return HandleResult(await _mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => HandleResult(await _mediator.Send(new DeleteDeviceCommand(id)));
}