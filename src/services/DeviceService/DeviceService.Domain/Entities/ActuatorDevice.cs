using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Entities;

public class ActuatorDevice : Device
{
    public ActuatorType ActuatorType { get; set; }
    public bool PowerState { get; set; }
}
