using AutoMapper;
using IoT.Application.Commands.Telemetry.ProcessTelemetry;
using IoT.Contracts.Telemetry;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/telemetry")]
public class TelemetryController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TelemetryController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Process([FromBody] ProcessTelemetryRequest request)
    {
        var command = _mapper.Map<ProcessTelemetryCommand>(request);
        return HandleResult(await _mediator.Send(command));
    }
}

