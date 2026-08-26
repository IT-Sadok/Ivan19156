using DeviceService.Application.Commands.CreateDevice;
using DeviceService.Application.Commands.DeleteDevice;
using DeviceService.Application.Commands.GenerateApiKey;
using DeviceService.Application.Commands.UpdateDevice;
using DeviceService.Application.Queries.GetDeviceApiKeys;
using DeviceService.Application.Queries.GetDeviceById;
using DeviceService.Application.Queries.GetDevices;
using DeviceService.Domain.Enums;
using IoT.Contracts.Devices;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Tech debt — DeviceService.Rest depends on IoT.Domain via IoT.Contracts.
// IoT.Contracts uses IoT.Domain.Enums directly (CreateDeviceRequest, UpdateDeviceRequest).
// Fix: move enums to IoT.Contracts independently from IoT.Domain.
using IoTDeviceType = IoT.Domain.Enums.DeviceType;
using IoTDeviceAdminStatus = IoT.Domain.Enums.DeviceAdminStatus;

namespace DeviceService.Rest.Controllers;

[Route("api/devices")]
[Authorize]
public class DevicesController : BaseController
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DeviceType? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetDevicesQuery(page, pageSize, type);
        return HandleResult(await _mediator.SendAsync<GetDevicesQuery, Result<PagedResult<DeviceResponse>>>(query));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetDeviceByIdQuery(id);
        return HandleResult(await _mediator.SendAsync<GetDeviceByIdQuery, Result<DeviceResponse>>(query));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
    {
        DeviceType deviceType = request.Type switch
        {
            IoTDeviceType.Sensor => DeviceType.Sensor,
            IoTDeviceType.Actuator => DeviceType.Actuator,
            _ => DeviceType.Sensor
        };

        var command = new CreateDeviceCommand(request.Name, deviceType, request.ManufacturerId);
        return HandleResult(await _mediator.SendAsync<CreateDeviceCommand, Result<Guid>>(command));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceRequest request)
    {
        DeviceAdminStatus adminStatus = (DeviceAdminStatus)(int)request.AdminStatus;
        var command = new UpdateDeviceCommand(id, request.Name, adminStatus, request.ManufacturerId);
        return HandleResult(await _mediator.SendAsync<UpdateDeviceCommand, Result<bool>>(command));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteDeviceCommand(id);
        return HandleResult(await _mediator.SendAsync<DeleteDeviceCommand, Result<bool>>(command));
    }

    [HttpPost("{id:guid}/api-keys")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GenerateApiKey(Guid id)
    {
        var command = new GenerateApiKeyCommand(id);
        return HandleResult(await _mediator.SendAsync<GenerateApiKeyCommand, Result<string>>(command));
    }

    [HttpGet("{id:guid}/api-keys")]
    public async Task<IActionResult> GetApiKeys(Guid id)
    {
        var query = new GetDeviceApiKeysQuery(id);
        return HandleResult(await _mediator.SendAsync<GetDeviceApiKeysQuery, Result<IEnumerable<ApiKeyDto>>>(query));
    }
}
