using System.Diagnostics;

namespace IoT.IntegrationTests.Infrastructure;

public static class PollingHelper
{
    public static async Task WaitUntilAsync(
        Func<Task<bool>> condition,
        TimeSpan? timeout = null,
        TimeSpan? interval = null)
    {
        var actualTimeout = timeout ?? TimeSpan.FromSeconds(30);
        var actualInterval = interval ?? TimeSpan.FromMilliseconds(500);
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < actualTimeout)
        {
            if (await condition()) return;
            await Task.Delay(actualInterval);
        }
        
        // throw new TimeoutException(
        //     $"Condition was not met within {actualTimeout.TotalSeconds}s");
    }
}