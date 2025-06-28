
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
using itbusina.sonar;

// Programmatic NLog configuration
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

static string? GetEnvironmentVariable(string name, bool required = true)
{
    if (required)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new InvalidOperationException($"{name} environment variable is not set.");
    }

    return Environment.GetEnvironmentVariable(name);
}

// Register SonarService with API version and connection details from environment variables
builder.Services.AddSingleton<SonarClient>(sp =>
{
    var host = GetEnvironmentVariable("SONAR_HOST") ?? throw new ArgumentNullException("{SONAR_HOST} is not set.");
    var token = GetEnvironmentVariable("SONAR_TOKEN", false);

    var logger = sp.GetRequiredService<ILogger<SonarClient>>();
    return new SonarClient(host, token);
});


var app = builder.Build();

await app.RunAsync();