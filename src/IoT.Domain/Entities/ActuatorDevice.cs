// IoT.Domain/Entities/ActuatorDevice.cs
using IoT.Domain.Enums;

namespace IoT.Domain.Entities;

public class ActuatorDevice : Device
{
    public ActuatorType ActuatorType { get; set; }
    public bool PowerState { get; set; }
}