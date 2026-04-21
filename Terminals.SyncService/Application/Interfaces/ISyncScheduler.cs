using Calabonga.OperationResults;

namespace Terminals.SyncService.Application.Interfaces;

/// <summary>
/// Service for scheduling synchronization
/// </summary>
public interface ISyncScheduler
{
    /// <summary>
    /// Waits until the next scheduled time
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<Operation<bool,string>> WaitUntilNextScheduleAsync(CancellationToken cancellationToken);
}
