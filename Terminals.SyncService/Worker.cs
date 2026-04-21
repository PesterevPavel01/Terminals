using Terminals.SyncService.Application.Interfaces;

namespace Terminals.SyncService;

/// <summary>
/// Background service for periodic synchronization of terminal data
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISyncManager _syncManager;
    private readonly ISyncScheduler _syncScheduler;

    public Worker( 
        ILogger<Worker> logger, 
        ISyncManager syncManager,
        ISyncScheduler syncScheduler)
    {
        _logger = logger;
        _syncManager = syncManager;
        _syncScheduler = syncScheduler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var waitResult = await _syncScheduler.WaitUntilNextScheduleAsync(stoppingToken);

            if (!waitResult.Ok)
            {
                _logger.LogInformation("Worker stopping due to cancellation during wait");
                break;
            }
            
            if (stoppingToken.IsCancellationRequested)
                break;

            var syncResult = await _syncManager.SyncAsync(stoppingToken);

            if (!syncResult.Ok)
            {
                _logger.LogError("Synchronization failed: {Error}", syncResult.Error);
            }
            else
            {
                _logger.LogInformation("Synchronization completed successfully");
            }
        }

        _logger.LogInformation("Worker stopped");
    }
}
