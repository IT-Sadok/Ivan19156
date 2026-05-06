namespace IoT.Interfaces.Services;

public interface ICommandHubNotifier
{
    Task NotifyCommandAsync(Guid deviceId, object command);
}
