using IoT.Domain.Enums;

namespace IoT.Domain.Entities;

public class SensorDevice : Device
{
    public SensorType SensorType { get; set; }
    public string? MeasurementUnit { get; set; }
}