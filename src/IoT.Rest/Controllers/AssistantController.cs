using IoT.Application.Queries.Assistant.ProcessAssistantQuery;
using IoT.Contracts.Assistant;
using IoT.Domain.Constants;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

//[Authorize(Roles = $"{Roles.Admin},{Roles.Technician}")]
public class AssistantController(IMediator mediator) : BaseController
{
    [HttpPost("query")]
    public async Task<IActionResult> Query(
        [FromBody] AssistantQueryRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(new ProcessAssistantQuery(request.Query), ct);
        return result.IsSuccess
            ? Ok(new AssistantQueryResponse(result.Value))
            : BadRequest(result.Error);
    }
}