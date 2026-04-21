using Calabonga.OperationResults;
using Infrastructure.Contracts.Enums;
using System.Text.Json;
using Terminals.Contracts.Dto;
using Terminals.Contracts.Interfaces;

namespace Terminals.Contracts.Entities;

public sealed class Office
{
    public const short CodesMaxLength = 50;

    public const int WorkTimeLength = 8192;

    public const string DefaultCountryCode = "Ru";
    protected Office( int id)
    {
        Id = id;
    }

    public int Id { get; private set; }
    public string? Code { get; private set; }
    public int CityCode { get; private set; }
    public string? Uuid { get; private set; }
    public OfficeType? Type { get; private set; }
    public string CountryCode { get; private set; }
    public string? AddressRegion { get; private set; }
    public string? AddressCity { get; private set; }
    public string? AddressStreet { get; private set; }
    public string? AddressHouseNumber { get; private set; }
    public int? AddressApartment { get; private set; }
    public string WorkTime { get; private set; }

    public Coordinates Coordinates { get; private set; }
    public Phone Phones { get; private set; }

    public static Operation<Office, string> Create(
        string id,
        Phone phones,
        Coordinates coordinates,
        WorktablesDto? worktables,
        string? code,
        int? cityCode,
        string? uuid,
        string? countryCode)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Operation.Error("Office id not found!");

        if(!int.TryParse(id, out _))
            return Operation.Error("Office id must be numeric");

        if (worktables is null)
            return Operation.Error("Work tables not found!");

        var office = new Office(int.Parse(id));

        if (!string.IsNullOrWhiteSpace(code))
            office.Code = code;

        if(cityCode is null)
            return Operation.Error("City code not found!");
        
        office.CityCode = (int)cityCode;

        if (!string.IsNullOrWhiteSpace(uuid))
            office.Uuid = uuid;

        office.CountryCode = string.IsNullOrWhiteSpace(countryCode) ? DefaultCountryCode : countryCode;

        office.Phones = phones;

        office.Coordinates = coordinates;

        var setWorkTimeResult = office.SetWorkTime(worktables);

        if (!setWorkTimeResult.Ok)
            return Operation.Error(setWorkTimeResult.Error);

        return office;
    }

    public Operation<bool, string> SetAddress(string? fullAddress, IAddressParser addressParser) 
    {
        if (fullAddress is null)
            return true;

        var result = addressParser.Pars(fullAddress);

        if (!result.Ok)
            return Operation.Error(result.Error);

        AddressRegion = result.Result.AddressRegion;

        AddressCity = result.Result.AddressCity;

        AddressStreet = result.Result.AddressStreet;

        AddressHouseNumber = result.Result.AddressHouseNumber;

        AddressApartment = result.Result.AddressApartment;

        return true;
    }

    public Operation<bool, string> SetType(bool isPvz, bool isPostamat, bool isWarehouse)
    {
        if (!isPvz && !isPostamat && !isWarehouse)
            return true;

        int trueCount = (isPvz ? 1 : 0) + (isPostamat ? 1 : 0) + (isWarehouse ? 1 : 0);

        if (trueCount > 1)
            return Operation.Error("Office cannot have multiple types simultaneously");

        if (isPvz)
            Type = OfficeType.PVZ;
        else if (isPostamat)
            Type = OfficeType.POSTAMAT;
        else if(isWarehouse)
            Type = OfficeType.WAREHOUSE;

        return true;
    }

    private Operation<bool, string> SetWorkTime(WorktablesDto worktables)
    {
        if (worktables is null)
            return Operation.Error("Work tables not found!");

        WorkTime = JsonSerializer.Serialize(worktables);

        return true;
    }
}
