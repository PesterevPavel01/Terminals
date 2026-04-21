using Calabonga.OperationResults;
using Terminals.SyncService.Application.Interfaces;

namespace Terminals.SyncService.Application
{
    public sealed class SyncManager : ISyncManager
    {
        private readonly IExistingDataCleanExecutor _existingDataCleanExecutor;
        private readonly IReadFromFileExecutor _readFromFileExecutor;
        private readonly IWriteToDBExecutor _writeToDBExecutor;
        private readonly ILogger<SyncManager> _logger;

        public SyncManager(IWriteToDBExecutor writeToDB, IExistingDataCleanExecutor existingDataClean, IReadFromFileExecutor readFromFile, ILogger<SyncManager> logger)
        {
            _writeToDBExecutor = writeToDB;
            _logger = logger;
            _existingDataCleanExecutor = existingDataClean;
            _readFromFileExecutor = readFromFile;
        }

        public async Task<Operation<bool, string>> SyncAsync(CancellationToken cancellationToken)
        {
            var cleanResult = await _existingDataCleanExecutor.ExecuteAsync(cancellationToken);

            if (!cleanResult.Ok)
            {
                _logger.LogError("Ошибка импорта: {Error}", cleanResult.Error);

                return Operation.Error($"Failed to clean existing records: {cleanResult.Error}");
            }

            _logger.LogInformation("Удалено {OldCount} старых записей", cleanResult.Result);

            var readResult = await _readFromFileExecutor.ExecuteAsync(cancellationToken);

            if (!readResult.Ok)
            {
                _logger.LogError("Ошибка импорта: {Error}", readResult.Error);

                return Operation.Error($"Failed to read data from file: {readResult.Error}");
            }

            var count = readResult.Result.City?.Sum(x => x.Terminals?.Terminal?.Count ?? 0) ?? 0;

            _logger.LogInformation("Загружено {Count} терминалов из JSON", count);

            var writeResult = await _writeToDBExecutor.ExecuteAsync(readResult.Result, cancellationToken);

            if (!writeResult.Ok)
            {
                foreach (var error in writeResult.Error)
                    _logger.LogError("Ошибка импорта: {Error}", error);

                return Operation.Error($"Failed to save to database. Errors count: {writeResult.Error.Count}");
            }

            _logger.LogInformation("Сохранено {NewCount} новых терминалов", writeResult.Result);

            return true;
        }
    }
}
