using Microsoft.Extensions.DependencyInjection;
using Terminals.Contracts.Interfaces;

namespace Terminals.AddressParser.Extensions;

public static class Services
{
    public static void AddDefaultAdressParser(this IServiceCollection services)
    {
        services.AddSingleton<IAddressParser, DefaultAddressParser>();
    }
}