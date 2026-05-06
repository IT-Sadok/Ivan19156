using AutoMapper;
using IoT.Application.Commands.Devices.CreateDevice;
using IoT.Application.Commands.Devices.DeleteDevice;
using IoT.Application.Commands.Devices.UpdateDevice;
using IoT.Application.Queries.Devices.GetDeviceById;
using IoT.Application.Queries.Devices.GetDevices;
using IoT.Contracts.Devices;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/devices")]
public class DevicesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DevicesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetDevicesFilterDto filter)
        => HandleResult(await _mediator.Send(
            new GetDevicesQuery(
                filter.Page,
                filter.PageSize,
                filter.Type,
                filter.AdminStatus,
                filter.ManufacturerId)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => HandleResult(await _mediator.Send(new GetDeviceByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
    {
        var command = _mapper.Map<CreateDeviceCommand>(request);
        var result = await _mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Value?.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceRequest request)
    {
        var command = _mapper.Map<UpdateDeviceCommand>(request) with { Id = id };
        return HandleResult(await _mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => HandleResult(await _mediator.Send(new DeleteDeviceCommand(id)));
}
