using Calabonga.OperationResults;
using Terminals.Contracts.Dto;
using Terminals.Contracts.Interfaces;

namespace Terminals.WorkTimeParser;

/// <summary>
/// Default implementation of work time parser
/// </summary>
public class DefaultWorkTimeParser : IWorkTimeParser
{
    /// <summary>
    /// Extracts work schedule from worktables data
    /// </summary>
    /// <param name="worktables">Worktables DTO containing schedule information</param>
    /// <returns>Work schedule string or error message</returns>
    public Operation<string, string> Pars(WorktablesDto? worktables)
    {
        if (worktables?.Worktable == null || !worktables.Worktable.Any())
        {
            return Operation.Error("worktables is null or empty!");
        }

        var timetable = worktables.Worktable.FirstOrDefault()?.Timetable;

        return timetable ?? string.Empty;
    }
}
