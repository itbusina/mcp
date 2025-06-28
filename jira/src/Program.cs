
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
using System.Net.Http.Headers;
using System.Text;

// Programmatic NLog configuration
var nlogConfig = new LoggingConfiguration();
var logfile = new FileTarget("logfile") { FileName = "logs/app.log", Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception}", CreateDirs = true };
nlogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
LogManager.Configuration = nlogConfig;
var nlogLogger = LogManager.GetCurrentClassLogger();

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

// Read environment variables for Jira configuration
var jiraHost = Environment.GetEnvironmentVariable("JIRA_HOST") ?? throw new InvalidOperationException("JIRA_HOST environment variable is not set.");
var jiraUser = Environment.GetEnvironmentVariable("JIRA_USER");
var jiraToken = Environment.GetEnvironmentVariable("JIRA_TOKEN") ?? throw new InvalidOperationException("JIRA_TOKEN environment variable is not set.");
var jiraAuthType = Environment.GetEnvironmentVariable("JIRA_AUTH_TYPE")?.ToLower() ?? "bearer"; // default to bearer

// Log the configuration values
var loggerFactory = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("Program");
logger.LogInformation("Using JIRA_HOST: {JiraHost}", jiraHost);
logger.LogInformation("Using JIRA_AUTH_TYPE: {JiraAuthType}", jiraAuthType);
logger.LogInformation("Using JIRA_USER: {JiraUser}", jiraUser);
logger.LogInformation("Using JIRA_TOKEN: {JiraToken}", jiraToken);

// Register a single HttpClient for Jira, selecting auth type based on env var
builder.Services.AddSingleton(_ =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(jiraHost)
    };
    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("jira-mcp", "1.0"));
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    if (jiraAuthType == "basic")
    {
        if (string.IsNullOrEmpty(jiraUser))
            throw new InvalidOperationException("JIRA_USER environment variable must be set for basic authentication.");

        var byteArray = Encoding.ASCII.GetBytes($"{jiraUser}:{jiraToken}");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }
    else if (jiraAuthType == "bearer")
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jiraToken);
    }
    else
    {
        throw new InvalidOperationException($"Unknown JIRA_AUTH_TYPE: {jiraAuthType}. Supported values are 'basic' or 'bearer'.");
    }

    return httpClient;
});

// Register JiraService
builder.Services.AddSingleton<jira.JiraService>();

var app = builder.Build();

await app.RunAsync();