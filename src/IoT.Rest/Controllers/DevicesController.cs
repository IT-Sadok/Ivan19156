using IoT.Application.Commands.Devices.CreateDevice;
using IoT.Application.Commands.Devices.DeleteDevice;
using IoT.Application.Commands.Devices.GenerateApiKey;
using IoT.Application.Commands.Devices.UpdateDevice;
using IoT.Application.Common.Mappings;
using IoT.Application.Queries.Devices.GetDeviceById;
using IoT.Application.Queries.Devices.GetDevices;
using IoT.Contracts.Devices;
using IoT.Domain.Constants;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/devices")]
public class DevicesController : BaseController
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetDevicesFilter filter)
        => HandleResult(await _mediator.SendAsync<GetDevicesQuery, Result<PagedResult<DeviceResponse>>>(
            new GetDevicesQuery(
                filter.Page,
                filter.PageSize,
                filter.Type,
                filter.AdminStatus,
                filter.ManufacturerId)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => HandleResult(await _mediator.SendAsync<GetDeviceByIdQuery, Result<DeviceResponse>>(
            new GetDeviceByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
    {
        var result = await _mediator.SendAsync<CreateDeviceCommand, Result<DeviceResponse>>(
            request.ToCommand());
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Value?.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceRequest request)
        => HandleResult(await _mediator.SendAsync<UpdateDeviceCommand, Result<DeviceResponse>>(
            request.ToCommand() with { Id = id }));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => HandleResult(await _mediator.SendAsync<DeleteDeviceCommand, Result<bool>>(
            new DeleteDeviceCommand(id)));

    [Authorize(Roles = Roles.Admin)]
    [HttpPost("{id:guid}/api-keys")]
    public async Task<IActionResult> GenerateApiKey(Guid id)
        => HandleResult(await _mediator.SendAsync<GenerateApiKeyCommand, Result<string>>(
            new GenerateApiKeyCommand(id)));
}