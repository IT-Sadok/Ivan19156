using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.CreateDevice;

public class CreateDeviceCommandHandler
    : IRequestHandler<CreateDeviceCommand, Result<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DeviceDto>> Handle(
        CreateDeviceCommand request,
        CancellationToken ct = default)
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            ManufacturerId = request.ManufacturerId,
            AdminStatus = DeviceAdminStatus.Active
        };

        await _unitOfWork.Devices.AddAsync(device);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<DeviceDto>.Success(_mapper.Map<DeviceDto>(device));
    }
}
