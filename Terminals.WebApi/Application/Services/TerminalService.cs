using Calabonga.OperationResults;
using Microsoft.EntityFrameworkCore;
using Terminals.WebApi.Application.Interfaces;
using Terminals.Contracts.Entities;
using Terminals.Infrastructure;

namespace Terminals.WebApi.Application.Services;

public class TerminalService : ITerminalService
{
    private readonly DellinDictionaryDbContext _dbContext;

    public TerminalService(DellinDictionaryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Operation<List<Office>, string>> GetOfficesByCityAndRegionAsync(string city, string region, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.Set<Office>()
                .Where(x => x.AddressRegion == region && x.AddressCity == city)
                .ToListAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            return Operation.Error(exception.Message);
        }
    }

    public async Task<Operation<int, string>> GetCityIdByCityAndRegionAsync(string city, string region, CancellationToken cancellationToken)
    {
        Office? office;

        try
        {
            office = await _dbContext.Set<Office>()
                .FirstOrDefaultAsync(x => x.AddressRegion == region && x.AddressCity == city, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            return Operation.Error(exception.Message);
        }

        if(office is null)
            return Operation.Error("City not found!");

        return office.CityCode;


    }
}
