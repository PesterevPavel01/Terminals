using Calabonga.OperationResults;
using Terminals.Contracts.Dto;

namespace Terminals.SyncService.Application.Interfaces;

/// <summary>
/// Executor for writing terminal data to database
/// </summary>
public interface IWriteToDBExecutor
{
    /// <summary>
    /// Executes the database write operation
    /// </summary>
    /// <param name="CityTerminals">DTO containing city and terminal data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result containing count of saved records or list of error messages</returns>
    Task<Operation<int, List<string>>> ExecuteAsync(CityTerminalsDto CityTerminals, CancellationToken cancellationToken);
}
