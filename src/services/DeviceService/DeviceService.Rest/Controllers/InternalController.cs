using DeviceService.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.Rest.Controllers;

[Route("api/internal")]
public class InternalController : BaseController
{
    private readonly IApiKeyService _apiKeyService;

    public InternalController(IApiKeyService apiKeyService)
        => _apiKeyService = apiKeyService;

    [HttpGet("api-keys/validate")]
    public async Task<IActionResult> ValidateApiKey(CancellationToken ct)
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
            return Unauthorized();

        var deviceId = await _apiKeyService.ValidateAsync(apiKey!, ct);
        if (deviceId is null)
            return Unauthorized();

        return Ok(new { DeviceId = deviceId.Value });
    }
}
