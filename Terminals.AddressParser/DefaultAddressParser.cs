using Calabonga.OperationResults;
using Terminals.Contracts.Dto;
using Terminals.Contracts.Interfaces;

namespace Terminals.AddressParser
{
    public class DefaultAddressParser : IAddressParser
    {
        public Operation<FullAddressDto, string> Pars(string fullAddress)
        {
            return new FullAddressDto() {
                AddressApartment = null,
                AddressCity = null,
                AddressHouseNumber = null,
                AddressRegion = null,
                AddressStreet = null
            };
        }
    }
}
