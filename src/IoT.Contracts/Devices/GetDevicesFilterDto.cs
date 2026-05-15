namespace IoT.Contracts.Devices;

public record GetDevicesFilter(
    int Page = 1,
    int PageSize = 20,
    int? Type = null,
    int? AdminStatus = null,
    Guid? ManufacturerId = null
);
