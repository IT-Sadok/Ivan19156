namespace IoT.Contracts.Devices;

public record GetDevicesFilterDto(
    int Page = 1,
    int PageSize = 20,
    int? Type = null,
    int? AdminStatus = null,
    Guid? ManufacturerId = null
);
