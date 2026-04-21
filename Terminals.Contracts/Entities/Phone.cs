using Calabonga.OperationResults;
using System.Text.Json.Serialization;
using Terminals.Contracts.Dto;

namespace Terminals.Contracts.Entities;

public class Phone
{
    public const short PhoneMaxLength = 20;

    protected Phone(){}

    public int Id { get; set; }

    public string PhoneNumber { get; private set; }

    public string? Additional { get; private set; }

    [JsonIgnore]
    public Office Office { get; private set; }

    [JsonIgnore]
    public int OfficeId { get; private set; }

    public static Operation<Phone, string> Create(string? mainPhone = null, List<PhoneDto>? phones = null)
    {
        if (string.IsNullOrWhiteSpace(mainPhone) && phones is null)
            return Operation.Error("Phone not found!");

        var phone = new Phone();

        if (!string.IsNullOrWhiteSpace(mainPhone))
        {
            phone.PhoneNumber = mainPhone;
        }
        else
        {
            var primaryPhone = phones.FirstOrDefault(x => x.Primary);

            if (primaryPhone is null)
                return Operation.Error("Primary phone not found!");

            phone.PhoneNumber = primaryPhone.Number;
        }

        if (phones is not null)
        {
            var additionalPhone = phones.FirstOrDefault(x => !x.Primary);

            if (additionalPhone is not null)
                phone.Additional = additionalPhone.Number;
        }

        return phone;
    }
}
