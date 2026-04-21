using Calabonga.OperationResults;
using Terminals.Contracts.Dto;

namespace Terminals.Contracts.Interfaces;

public interface IAddressParser
{
    Operation<FullAddressDto, string> Pars(string fullAddress);
}