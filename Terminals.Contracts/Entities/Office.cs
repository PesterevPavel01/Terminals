using Calabonga.OperationResults;
using Infrastructure.Contracts.Enums;
using Terminals.Contracts.Dto;

namespace Terminals.Contracts.Entities;

public sealed class Office
{
    public const short CodesMaxLength = 50;

    public const int WorkTimeMaxLength = 255;

    public const int AddressRegionMaxLength = 255;

    public const int AddressCityMaxLength = 100;

    public const int AddressStreetMaxLength = 255;

    public const int AddressHouseNumberMaxLength = 100;

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
        string worktTime,
        string? code,
        int? cityCode,
        string? uuid,
        string? countryCode)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Operation.Error("Office id not found!");

        if(!int.TryParse(id, out _))
            return Operation.Error("Office id must be numeric");

        var office = new Office(int.Parse(id));

        if (!string.IsNullOrWhiteSpace(code))
            office.Code = code;

        if(cityCode is null)
            return Operation.Error("City code not found!");

        office.CityCode = (int)cityCode;

        if (string.IsNullOrWhiteSpace(worktTime))
            return Operation.Error("work time not found!");

        if (!string.IsNullOrWhiteSpace(uuid))
            office.Uuid = uuid;

        office.CountryCode = string.IsNullOrWhiteSpace(countryCode) ? DefaultCountryCode : countryCode;

        office.Phones = phones;

        office.Coordinates = coordinates;

        office.WorkTime = worktTime;

        return office;
    }

    public Operation<bool, string> SetAddress(FullAddressDto fullAdress) 
    {
        AddressRegion = fullAdress.AddressRegion;

        AddressCity = fullAdress.AddressCity;

        AddressStreet = fullAdress.AddressStreet;

        AddressHouseNumber = fullAdress.AddressHouseNumber;

        AddressApartment = fullAdress.AddressApartment;

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
}
