
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
LogManager.Configuration = new LoggingConfiguration
{
    LoggingRules =
    {
        new LoggingRule("*", NLog.LogLevel.Info, new FileTarget("logfile")
        {
            FileName = "logs/app.log",
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

// Register a single HttpClient for Jira, reading environment variables inside the registration
builder.Services.AddSingleton(_ =>
{
    var jiraHost = Environment.GetEnvironmentVariable("JIRA_HOST") ?? throw new InvalidOperationException("JIRA_HOST environment variable is not set.");
    var jiraUser = Environment.GetEnvironmentVariable("JIRA_USER");
    var jiraToken = Environment.GetEnvironmentVariable("JIRA_TOKEN") ?? throw new InvalidOperationException("JIRA_TOKEN environment variable is not set.");
    var jiraAuthType = Environment.GetEnvironmentVariable("JIRA_AUTH_TYPE")?.ToLower() ?? "bearer";

    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(jiraHost)
    };
    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("jira-mcp", "1.0"));
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    httpClient.DefaultRequestHeaders.Authorization = jiraAuthType switch
    {
        "basic" => string.IsNullOrEmpty(jiraUser)
            ? throw new InvalidOperationException("JIRA_USER environment variable must be set for basic authentication.")
            : new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{jiraUser}:{jiraToken}"))),
        "bearer" => new AuthenticationHeaderValue("Bearer", jiraToken),
        _ => throw new InvalidOperationException($"Unknown JIRA_AUTH_TYPE: {jiraAuthType}. Supported values are 'basic' or 'bearer'.")
    };

    return httpClient;
});

// Register JiraService
builder.Services.AddSingleton<jira.JiraService>();

var app = builder.Build();

await app.RunAsync();