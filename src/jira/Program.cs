
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
using itbusina.jira;

// Programmatic NLog configuration with log file path from environment variable (inline)
LogManager.Configuration = new LoggingConfiguration
{
    LoggingRules =
    {
        new LoggingRule("*", NLog.LogLevel.Info, new FileTarget("logfile")
        {
            FileName = Environment.GetEnvironmentVariable("JIRA_LOG_FILE") ?? "logs/app.log",
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

// Register JiraService with API version and connection details from environment variables
builder.Services.AddSingleton<JiraService>(sp =>
{
    var jiraHost = Environment.GetEnvironmentVariable("JIRA_HOST") ?? throw new InvalidOperationException("JIRA_HOST environment variable is not set.");
    var jiraUser = Environment.GetEnvironmentVariable("JIRA_USER");
    var jiraToken = Environment.GetEnvironmentVariable("JIRA_TOKEN") ?? throw new InvalidOperationException("JIRA_TOKEN environment variable is not set.");
    var jiraAuthType = Environment.GetEnvironmentVariable("JIRA_AUTH_TYPE")?.ToLower() ?? "bearer";
    var apiVersion = Environment.GetEnvironmentVariable("JIRA_API_VERSION") ?? throw new InvalidOperationException("JIRA_API_VERSION environment variable is not set.");
    
    var logger = sp.GetRequiredService<ILogger<JiraService>>();
    return new JiraService(logger, jiraHost, jiraUser, jiraToken, jiraAuthType, apiVersion);
});

var app = builder.Build();

await app.RunAsync();