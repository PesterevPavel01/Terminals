using System.Text.Json.Serialization;

namespace Terminals.Contracts.Dto;

public sealed class PhoneDto
{
    [JsonPropertyName("number")]
    public required string Number { get; set; }

    [JsonPropertyName("primary")]
    public bool Primary { get; set; }
}