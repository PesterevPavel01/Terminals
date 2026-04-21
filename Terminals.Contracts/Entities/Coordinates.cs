using Calabonga.OperationResults;
using System.Text.RegularExpressions;

namespace Terminals.Contracts.Entities;

public class Coordinates
{
    protected Coordinates(){}

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public static Operation<Coordinates, String> Create(string latitude, string longitude) 
    {
        var coordinates = new Coordinates();

        if (String.IsNullOrWhiteSpace(latitude))
            return Operation.Error("Latitude is null or White Space");

        if (String.IsNullOrWhiteSpace(longitude))
            return Operation.Error("Longitude is null or White Space");

        if (!IsValidNumberString(latitude) || !IsValidNumberString(longitude))
            return Operation.Error("Invalid characters in coordinate");

        coordinates.Latitude = Convert.ToDouble(latitude.Replace('.', ','));
        coordinates.Longitude = Convert.ToDouble(longitude.Replace('.', ','));

        return coordinates;
    }

    private static bool IsValidNumberString(string value)
    {
        foreach (char c in value)
        {
            if (!char.IsDigit(c) && c != '.' && c != ',' && c != '-')
                return false;
        }
        return true;
    }
}
