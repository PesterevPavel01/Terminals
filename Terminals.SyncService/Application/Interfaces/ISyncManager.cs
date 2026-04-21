using Calabonga.OperationResults;

namespace Terminals.SyncService.Application.Interfaces;

/// <summary>
/// Manager that orchestrates the synchronization process
/// </summary>
public interface ISyncManager
{
    /// <summary>
    /// Executes the full synchronization process
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result containing success status or error message</returns>
    Task<Operation<bool, string>> SyncAsync(CancellationToken cancellationToken);
}
