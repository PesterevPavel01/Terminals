using Calabonga.OperationResults;
using Terminals.Contracts.Entities;

namespace Terminals.WebApi.Application.Interfaces;

public interface ITerminalService
{
    /// <summary>
    /// Search for offices/terminals by city and region name
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="region">Region name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result containing list of offices or error message</returns>
    Task<Operation<List<Office>,string>> GetOfficesByCityAndRegionAsync(string city, string region, CancellationToken cancellationToken);

    /// <summary>
    /// Search for city ID by city and region name
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="region">Region name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result containing city ID or error message</returns>
    Task<Operation<int, string>> GetCityIdByCityAndRegionAsync(string city, string region, CancellationToken cancellationToken);
}
