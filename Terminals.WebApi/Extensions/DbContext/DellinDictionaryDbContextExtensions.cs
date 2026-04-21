using Microsoft.EntityFrameworkCore;
using Npgsql;
using Terminals.Infrastructure;

namespace Terminals.WebApi.Extension.DbContext;
public static class DellinDictionaryDbContextExtensions
{
    public static void AddDellinDictionaryDbContext(this IServiceCollection services, IConfiguration configuration)
    { 
        var connectionString = configuration.GetConnectionString("DellinDictionaryDbConnectionString");

        services.AddDbContext<DellinDictionaryDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
    }
}
