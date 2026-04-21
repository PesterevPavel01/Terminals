using System.Text.Json.Serialization;

namespace Terminals.Contracts.Dto;

public sealed class TerminalDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("fullAddress")]
    public string? FullAddress { get; set; }

    [JsonPropertyName("latitude")]
    public required string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public required string Longitude { get; set; }

    [JsonPropertyName("isPVZ")]
    public bool IsPvz { get; set; }

    [JsonPropertyName("isPostamat")]
    public bool IsPostamat { get; set; }

    [JsonPropertyName("isWarehouse")]
    public bool IsWarehouse { get; set; }

    [JsonPropertyName("mainPhone")]
    public string MainPhone { get; set; }

    [JsonPropertyName("phones")]
    public List<PhoneDto> Phones { get; set; } = [];

    [JsonPropertyName("receiveCargo")]
    public bool ReceiveCargo { get; set; }

    [JsonPropertyName("giveoutCargo")]
    public bool GiveoutCargo { get; set; }

    [JsonPropertyName("calcSchedule")]
    public CalcScheduleDto CalcSchedule { get; set; }

    [JsonPropertyName("worktables")]
    public WorktablesDto? Worktables { get; set; }
}

