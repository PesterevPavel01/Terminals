using Microsoft.EntityFrameworkCore;
using Npgsql;
using Terminals.Infrastructure;

namespace Terminals.SyncService.Definitions.DbContext;
public static class DellinDictionaryDbContextExtensions
{
    public static void AddDellinDictionaryDbContext(this IServiceCollection services, IConfiguration configuration)
    { 
        var connectionString = configuration.GetConnectionString("DellinDictionaryDbConnectionString");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        dataSourceBuilder.EnableDynamicJson();

        var dataSource = dataSourceBuilder.Build();

        services.AddSingleton(dataSource);

        services.AddDbContext<DellinDictionaryDbContext>((serviceProvider, options) =>
        {
            var npgSqlDataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();

            options.UseNpgsql(npgSqlDataSource);
        },
        ServiceLifetime.Singleton);
    }
}
