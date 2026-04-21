using System.Text.Json.Serialization;

namespace Terminals.Contracts.Dto;

public sealed class CityDto()
{
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("cityID")]
    public int? Id { get; set; } = -1;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;


    [JsonPropertyName("terminals")]
    public TerminalsContainerDto Terminals { get; set; } = new();
}
