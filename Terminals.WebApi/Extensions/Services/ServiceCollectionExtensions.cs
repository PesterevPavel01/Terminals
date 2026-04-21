using Terminals.WebApi.Application.Interfaces;
using Terminals.WebApi.Application.Services;

namespace Terminals.WebApi.Extensions.Services;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITerminalService, TerminalService>();
    }
}
