using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Terminals.SyncService.Definitions.Logger;

public static class LoggerExtensions
{
    public static void AddStructuredLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();

        var configuration = new LoggerConfiguration()
            .Enrich.With(new CustomLevelEnricher())
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .WriteTo.Console(
                outputTemplate: "{Timestamp:HH:mm:ss} {CustomLevel}: {Message:lj}{NewLine}{Exception}",
                theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code
            )
            .CreateLogger();

        loggingBuilder.AddSerilog(configuration, dispose: true);
    }
}

public class CustomLevelEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var level = logEvent.Level switch
        {
            LogEventLevel.Verbose => "TRACE",
            LogEventLevel.Debug => "DEBUG",
            LogEventLevel.Information => "INFO",
            LogEventLevel.Warning => "WARN",
            LogEventLevel.Error => "ERROR",
            LogEventLevel.Fatal => "FATAL",
            _ => logEvent.Level.ToString().ToUpperInvariant()
        };

        var property = propertyFactory.CreateProperty("CustomLevel", level);
        logEvent.AddPropertyIfAbsent(property);
    }
}