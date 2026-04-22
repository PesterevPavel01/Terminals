using Calabonga.OperationResults;
using Microsoft.EntityFrameworkCore;
using Terminals.Contracts.Entities;
using Terminals.Infrastructure;
using Terminals.SyncService.Application.Interfaces;

namespace Terminals.SyncService.Application.Messages;

/// <summary>
/// Executor for cleaning existing data before synchronization
/// </summary>
public sealed class ExistingDataCleanExecutor : IExistingDataCleanExecutor
{
    private readonly DellinDictionaryDbContext _dbContext;

    public ExistingDataCleanExecutor(DellinDictionaryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Operation<int, string>> ExecuteAsync(CancellationToken cancellationToken)
    {
        int deletedCount;

        try
        {
            deletedCount = await _dbContext.Set<Office>().ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            return Operation.Error(exception.Message);
        }

        return deletedCount;
    }
}