using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton(_ =>
{
    var host = Environment.GetEnvironmentVariable("JIRA_HOST")
        ?? throw new InvalidOperationException("JIRA_HOST environment variable is not set.");
    var user = Environment.GetEnvironmentVariable("JIRA_USER")
        ?? throw new InvalidOperationException("JIRA_USER environment variable must be set.");
    var token = Environment.GetEnvironmentVariable("JIRA_TOKEN")
        ?? throw new InvalidOperationException("JIRA_TOKEN environment variable must be set.");

    // Create an HttpClient with the base address set to the JIRA host
    var client = new HttpClient() { BaseAddress = new Uri(host) };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("jira-mcp-tool", "1.0"));
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    // Add Basic Authentication using environment variables JIRA_USER and JIRA_TOKEN
    var authValue = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{token}"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

    return client;
});

var app = builder.Build();

await app.RunAsync();