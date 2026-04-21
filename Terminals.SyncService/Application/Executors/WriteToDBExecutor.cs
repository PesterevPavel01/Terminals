using Calabonga.OperationResults;
using EFCore.BulkExtensions;
using Terminals.Contracts.Dto;
using Terminals.Contracts.Entities;
using Terminals.Contracts.Interfaces;
using Terminals.Infrastructure;
using Terminals.SyncService.Application.Interfaces;

namespace Terminals.SyncService.Application.Messages;

public sealed class WriteToDBExecutor : IWriteToDBExecutor
{
    private readonly DellinDictionaryDbContext _dbContext;

    private readonly IAddressParser _addressParser;

    public WriteToDBExecutor(DellinDictionaryDbContext dbContext, IAddressParser addressParser)
    {
        _dbContext = dbContext;
        _addressParser = addressParser;
    }

    public async Task<Operation<int, List<string>>> ExecuteAsync(CityTerminalsDto cityTerminals, CancellationToken cancellationToken)
    {
        List<Office> offices = [];

        List<string> errors = [];

        var wasCancelled = false;

        foreach (var city in cityTerminals.City)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                wasCancelled = true;
                break;
            }

            foreach (var terminal in city.Terminals.Terminal)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    wasCancelled = true;
                    break;
                }

                var coordinatesResult = Coordinates.Create(terminal.Latitude, terminal.Longitude);

                if (!coordinatesResult.Ok)
                {
                    errors.Add($"Terminal {terminal.Id}. {coordinatesResult.Error}");
                    continue;
                }

                var phonesResult = Phone.Create(terminal.MainPhone, terminal.Phones);

                if (!phonesResult.Ok)
                {
                    errors.Add($"Terminal {terminal.Id}. {phonesResult.Error}");
                    continue;
                }

                var officeResult = Office.Create(
                    id: terminal.Id,
                    phones: phonesResult.Result,
                    coordinates: coordinatesResult.Result,
                    worktables: terminal.Worktables,
                    code: terminal.Code,
                    cityCode: city.Id,
                    uuid: terminal.Uuid,
                    countryCode: city.CountryCode);

                if (!officeResult.Ok)
                {
                    errors.Add($"Terminal {terminal.Id}. {officeResult.Error}");
                    continue;
                }

                var setTypeResult = officeResult.Result
                    .SetType(
                        isPvz: terminal.IsPvz, 
                        isPostamat: terminal.IsPostamat,
                        isWarehouse: terminal.IsWarehouse);

                if (!setTypeResult.Ok)
                {
                    errors.Add($"Terminal {terminal.Id}. {setTypeResult.Error}");
                    continue;
                }

                var addAddressResult = officeResult.Result.SetAddress(terminal.FullAddress, _addressParser);

                if (!addAddressResult.Ok)
                {
                    errors.Add($"Terminal {terminal.Id}. {addAddressResult.Error}");
                    continue;
                }

                offices.Add(officeResult.Result);
            }
        }

        if (wasCancelled)
        {
            errors.Add("Operation was partially completed due to shutdown request");
        }

        var bulkConfig = new BulkConfig
        {
            BatchSize = 1000,
            UseTempDB = true,
            TrackingEntities = false
        };

        try
        {
            await _dbContext.BulkInsertAsync(offices, bulkConfig, null, null, cancellationToken);
        }
        catch (Exception exception) 
        {
            errors.Add(exception.Message);
        }

        if (errors.Any())
            return Operation.Error(errors);

        return offices.Count;
    }
}
