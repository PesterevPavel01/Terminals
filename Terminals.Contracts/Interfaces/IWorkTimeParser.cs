using Calabonga.OperationResults;
using Terminals.Contracts.Dto;

namespace Terminals.Contracts.Interfaces;

/// <summary>
/// Provides functionality to parse work schedules from terminal worktables data
/// </summary>
public interface IWorkTimeParser
{
    /// <summary>
    /// Parses work schedule information from worktables DTO
    /// </summary>
    /// <param name="worktables">Worktables DTO containing schedule information</param>
    /// <returns>Operation result containing work schedule string or error message</returns>
    Operation<string, string> Pars(WorktablesDto? worktables);
}
