using Calabonga.OperationResults;

namespace Terminals.SyncService.Application.Interfaces;

/// <summary>
/// Executor for cleaning existing data before synchronization
/// </summary>
public interface IExistingDataCleanExecutor
{
    /// <summary>
    /// Executes the cleanup operation
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result containing number of deleted records or error message</returns>
    Task<Operation<int, string>> ExecuteAsync(CancellationToken cancellationToken);
}
