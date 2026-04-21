using Calabonga.OperationResults;
using System.Text.Json;
using Terminals.Contracts.Dto;
using Terminals.SyncService.Application.Interfaces;

namespace Terminals.SyncService.Application.Messages;

public sealed class ReadFromFileExecutor : IReadFromFileExecutor
{
    public async Task<Operation<CityTerminalsDto, string>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var homePath = AppContext.BaseDirectory;

        string filePath = Path.Combine(homePath, "files", "terminals.json");

        if (!File.Exists(filePath))
            return Operation.Error($"File not found: {filePath}");

        if (cancellationToken.IsCancellationRequested)
            return Operation.Error("Operation was cancelled before reading the file");

        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, true);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        CityTerminalsDto? result;

        try
        {
            result = await JsonSerializer.DeserializeAsync<CityTerminalsDto>(fileStream, options, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return Operation.Error("Operation was cancelled while reading the file");
        }
        catch (Exception exception)
        {
            return Operation.Error(exception.Message);
        }

        if (result is null || result.City is null || !result.City.Any())
            return Operation.Error("No cities found in the JSON file!");

        return result;
    }
}
