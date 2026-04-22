using Calabonga.OperationResults;
using Terminals.Contracts.Dto;

namespace Terminals.Contracts.Interfaces;

/// <summary>
/// Provides functionality to parse a full address string into its constituent components
/// </summary>
public interface IAddressParser
{
    /// <summary>
    /// Parses a full address string into structured address components
    /// </summary>
    /// <param name="fullAddress">Full address string to parse</param>
    /// <returns>Operation result containing parsed address components or error message</returns>
    Operation<FullAddressDto, string> Pars(string fullAddress);
}