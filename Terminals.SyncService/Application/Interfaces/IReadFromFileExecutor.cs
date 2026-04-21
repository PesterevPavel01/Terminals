using Calabonga.OperationResults;
using Terminals.Contracts.Dto;

namespace Terminals.SyncService.Application.Interfaces;

/// <summary>
/// Executor for reading terminal data from JSON file
/// </summary>
public interface IReadFromFileExecutor
{
    /// <summary>
    /// Executes the file read operation
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result containing CityTerminalsDto or error message</returns>
    Task<Operation<CityTerminalsDto, string>> ExecuteAsync(CancellationToken cancellationToken);
}
