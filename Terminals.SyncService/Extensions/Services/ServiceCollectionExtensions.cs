using Terminals.SyncService.Application;
using Terminals.SyncService.Application.Interfaces;
using Terminals.SyncService.Application.Messages;

namespace Terminals.SyncService.Definitions.Common;

public static class ServiceCollectionExtensions 
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ISyncManager, SyncManager>();
        services.AddSingleton<ISyncScheduler, SyncScheduler>();

        services.AddSingleton<IExistingDataCleanExecutor,ExistingDataCleanExecutor>();
        services.AddSingleton<IReadFromFileExecutor, ReadFromFileExecutor>();
        services.AddSingleton<IWriteToDBExecutor, WriteToDBExecutor>();
    }
}
