using Microsoft.Extensions.DependencyInjection;
using Terminals.Contracts.Interfaces;

namespace Terminals.WorkTimeParser.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultWorkTimeParser(this IServiceCollection services)
    {
        services.AddSingleton<IWorkTimeParser, DefaultWorkTimeParser>();
    }
}