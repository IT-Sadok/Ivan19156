using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Entities;

public class SensorDevice : Device
{
    public SensorType SensorType { get; set; }
    public string? MeasurementUnit { get; set; }
}
