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

    var token = Environment.GetEnvironmentVariable("JIRA_PAT")
        ?? throw new InvalidOperationException("JIRA_PAT environment variable must be set.");

    // Create an HttpClient with the base address set to the JIRA host
    var client = new HttpClient() { BaseAddress = new Uri(host) };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("jira-mcp", "1.0"));
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    // Add Bearer Authentication using the JIRA_PAT as a Personal Access Token (PAT)
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    return client;
});

var app = builder.Build();

await app.RunAsync();