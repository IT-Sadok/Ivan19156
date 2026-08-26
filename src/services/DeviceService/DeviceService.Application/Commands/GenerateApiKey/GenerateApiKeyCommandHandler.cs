using DeviceService.Interfaces;
using DeviceService.Interfaces.Services;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.GenerateApiKey;

public class GenerateApiKeyCommandHandler : IRequestHandler<GenerateApiKeyCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApiKeyService _apiKeyService;

    public GenerateApiKeyCommandHandler(IUnitOfWork unitOfWork, IApiKeyService apiKeyService)
    {
        _unitOfWork = unitOfWork;
        _apiKeyService = apiKeyService;
    }

    public async Task<Result<string>> ExecuteAsync(GenerateApiKeyCommand request, CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.DeviceId, ct);
        if (device is null)
            return Result<string>.NotFound($"Device {request.DeviceId} not found.");

        var key = await _apiKeyService.GenerateAsync(request.DeviceId, ct);
        return Result<string>.Success(key);
    }
}
