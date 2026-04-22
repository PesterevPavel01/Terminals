using Calabonga.OperationResults;
using Terminals.SyncService.Application.Interfaces;

namespace Terminals.SyncService.Application;

/// <summary>
/// Scheduler that calculates delay and waits until 02:00 MSK
/// </summary>
public sealed class SyncScheduler : ISyncScheduler
{
    private readonly ILogger<SyncScheduler> _logger;

    public SyncScheduler(ILogger<SyncScheduler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Waits until the next scheduled run time at 02:00 MSK
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for graceful shutdown</param>
    /// <returns>Operation result containing true if wait completed successfully, or error message if cancelled</returns>
    public async Task<Operation<bool,string>> WaitUntilNextScheduleAsync(CancellationToken cancellationToken)
    {
        var delay = GetDelayUntilNextRun();

        _logger.LogInformation($"Next synchronization scheduled in {delay:hh\\:mm}");

        try
        {
            await Task.Delay(delay, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            return Operation.Error("Waiting was cancelled");
        }

        return true;
    }

    /// <summary>
    /// Calculates the delay until the next scheduled run time (02:00 MSK)
    /// </summary>
    /// <returns>Time span until next execution</returns>
    private static TimeSpan GetDelayUntilNextRun()
    {
        var mskTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

        var currentTime = TimeZoneInfo
            .ConvertTime(DateTime.UtcNow, mskTimeZone)
            .TimeOfDay;

        var targetTime = new TimeSpan(2, 0, 0);

        TimeSpan delay;

        if (currentTime < targetTime)
        {
            delay = targetTime - currentTime;
        }
        else
        {
            delay = targetTime + TimeSpan.FromDays(1) - currentTime;
        }

        return delay;
    }
}
