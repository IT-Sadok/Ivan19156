using System.Net.Http.Json;
using TelemetryService.Interfaces.Services;

namespace TelemetryService.Infrastructure.Services;

public class DeviceServiceClient : IDeviceServiceClient
{
    private readonly HttpClient _httpClient;

    public DeviceServiceClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<Guid?> ValidateApiKeyAsync(string apiKey, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/internal/api-keys/validate");
        request.Headers.Add("X-Api-Key", apiKey);

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<ValidateApiKeyResponse>(ct);
        return result?.DeviceId;
    }

    private record ValidateApiKeyResponse(Guid DeviceId);
}
