using Calabonga.OperationResults;
using System.Text.RegularExpressions;
using Terminals.Contracts.Dto;
using Terminals.Contracts.Interfaces;

namespace Terminals.AddressParser;

/// <summary>
/// Default implementation of address parser for Russian postal addresses
/// </summary>
public class DefaultAddressParser : IAddressParser
{
    private static readonly HashSet<string> FederalCities = new(
        ["москва", "санкт-петербург", "севастополь"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> RegionIndicators = new(
        ["обл", "край", "республика", "р-н", "округ"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> CityIndicators = new(
        ["город" , "г.", "г"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> DistrictIndicators = new(
        ["р-н", "район", "мкр", "микрорайон", "пос", "поселок", "дер ", "деревня", "село"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> StreetIndicators = new(
        ["ул", "улица", "проспект", "пр-кт", "пр", "пер", "переулок", "ш", "шоссе", "б-р", "бульвар", "наб", "набережная", "аллея", "пл", "площадь"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> HouseIndicators = new(
        ["дом", "строение", "стр", "владение", "влд"],
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> ApartmentIndicators = new(
        ["кв", "квартира", "офис", "помещение", "пом", "оф"],
        StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Parses a full address string into its components
    /// </summary>
    /// <param name="fullAddress">Full address string to parse</param>
    /// <returns>Operation result containing parsed address components or error message</returns>
    public Operation<FullAddressDto, string> Pars(string fullAddress)
    {
        try
        {
            return ParseAddress(fullAddress);
        }
        catch (RegexMatchTimeoutException ex)
        {
            return Operation.Error($"{ex.Message}");
        }
    }

    /// <summary>
    /// Internal parsing logic
    /// </summary>
    private Operation<FullAddressDto, string> ParseAddress(string fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
            return Operation.Error("Full address is null or empty");

        var parts = fullAddress
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        if (!parts.Any())
            return Operation.Error("Address contains no data after splitting");

        var indexPart = parts.FirstOrDefault(x => Regex.IsMatch(x, @"^\d{6}$"));
        if (indexPart is not null)
            parts.Remove(indexPart);

        var result = new FullAddressDto
        {
            AddressRegion = string.Empty,
            AddressCity = string.Empty,
            AddressStreet = string.Empty,
            AddressHouseNumber = string.Empty,
            AddressApartment = null
        };

        #region Region

        var regionPart = parts.FirstOrDefault(x => RegionIndicators.Any(x.Contains));
        if (regionPart is not null)
        {
            result.AddressRegion = regionPart;
            parts.Remove(regionPart);
        }

        #endregion

        #region City

        var cityPart = parts.FirstOrDefault(x =>
            CityIndicators.Any(indicator =>
                Regex.IsMatch(x, $@"\b{Regex.Escape(indicator)}\b", RegexOptions.IgnoreCase)));

        if (cityPart is null)
            return Operation.Error("City not found!");

        var removePattern = $@"\s*\b({string.Join("|", CityIndicators)})\b\s*";

        var cityName = Regex.Replace(cityPart, removePattern, string.Empty, RegexOptions.IgnoreCase).Trim();
        result.AddressCity = cityName;
        
        parts.Remove(cityPart);

        if (string.IsNullOrEmpty(result.AddressRegion) && FederalCities.Contains(cityName))
            result.AddressRegion = cityName;

        #endregion

        #region District

        var districtParts = parts
            .Where(x => DistrictIndicators.Any(x.Contains))
            .ToList();

        if (districtParts.Any())
            foreach (var districtPart in districtParts)
                parts.Remove(districtPart);

        #endregion

        #region Street

        var streetPart = parts.FirstOrDefault(x => StreetIndicators.Any(x.Contains));
        if (streetPart is null)
            return Operation.Error("Street not found!");

        result.AddressStreet = streetPart;
        parts.Remove(streetPart);

        #endregion

        #region House

        var housePart = parts.FirstOrDefault(x =>
            HouseIndicators.Any(x.Contains) ||
            Regex.IsMatch(x, @"^\d+[А-Яа-я]?(?:[\/\\]\d+)?$"));

        if (housePart is null)
            return Operation.Error("House number not found!");

        var houseMatch = Regex.Match(housePart, @"(\d+(?:[А-Яа-я])?(?:[\/\\]\d+)?(?:[кк]\s*\.?\s*\d+)?)");
        result.AddressHouseNumber = houseMatch.Success ? houseMatch.Groups[1].Value : housePart;
        parts.Remove(housePart);

        #endregion

        #region Apartment

        if (parts.Any())
        {
            var apartmentPart = parts.FirstOrDefault(x =>
                ApartmentIndicators.Any(x.Contains));

            if (apartmentPart != null)
            {
                var aptMatch = Regex.Match(apartmentPart, @"(\d+)");
                if (aptMatch.Success)
                {
                    if (int.TryParse(aptMatch.Groups[1].Value, out var apartmentNumber))
                    {
                        result.AddressApartment = apartmentNumber;
                    }
                    else
                    {
                        return Operation.Error($"Failed to parse apartment number from: '{apartmentPart}'. Expected numeric value.");
                    }
                }
                parts.Remove(apartmentPart);
            }
        }

        #endregion

        if (parts.Any())
        {
            var remaining = string.Join(", ", parts);
            if (!string.IsNullOrEmpty(result.AddressHouseNumber))
                result.AddressHouseNumber = $"{result.AddressHouseNumber} {remaining}";
        }

        return result;
    }
}