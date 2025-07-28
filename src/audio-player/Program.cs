
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;

// Programmatic NLog configuration with log file path from environment variable (inline)
LogManager.Configuration = new LoggingConfiguration
{
    LoggingRules =
    {
        new LoggingRule("*", NLog.LogLevel.Info, new FileTarget("logfile")
        {
            FileName = Environment.GetEnvironmentVariable("LOG_FILE") ?? "logs/app.log",
            Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception}",
            CreateDirs = true
        })
    }
};

// Create a host builder with empty settings
var builder = Host.CreateEmptyApplicationBuilder(settings: null);

// Configure logging to use NLog
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    logging.AddNLog();
});

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

await app.RunAsync();