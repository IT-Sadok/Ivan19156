using DeviceService.Application.Commands.IssueDeviceCommand;
using DeviceService.Application.Queries.GetCommandTypeBySlug;
using DeviceService.Application.Queries.GetDeviceCommands;
using DeviceService.Domain.Enums;
using DeviceService.Interfaces;
using IoT.Contracts.DeviceCommands;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.Rest.Controllers;

[Route("api/devices/{deviceId:guid}/commands")]
[Authorize]
public class DeviceCommandsController : BaseController
{
    private readonly IMediator _mediator;

    public DeviceCommandsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Issue(Guid deviceId, [FromBody] CreateDeviceCommandRequest request, CancellationToken ct)
    {
        // TODO: Tech debt — double cast required because IoT.Contracts.CreateDeviceCommandRequest
        // uses IoT.Domain.Enums.CommandTypeSlug instead of DeviceService.Domain.Enums.CommandTypeSlug.
        // Fix: extract shared enums into IoT.Contracts independently from IoT.Domain.
        var slug = ((DeviceService.Domain.Enums.CommandTypeSlug)(int)request.CommandTypeSlug).ToSlug();
        var commandTypeResult = await _mediator.SendAsync<GetCommandTypeBySlugQuery, Result<Guid>>(
            new GetCommandTypeBySlugQuery(slug));

        if (!commandTypeResult.IsSuccess)
            return BadRequest($"Unknown command type: {request.CommandTypeSlug}");

        var command = new IssueDeviceCommandCommand(deviceId, commandTypeResult.Value, null, request.Parameters);
        return HandleResult(await _mediator.SendAsync<IssueDeviceCommandCommand, Result<Guid>>(command));
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid deviceId)
    {
        var query = new GetDeviceCommandsQuery(deviceId);
        return HandleResult(await _mediator.SendAsync<GetDeviceCommandsQuery, Result<IEnumerable<DeviceCommandResponse>>>(query));
    }
}
