using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton(_ =>
{
    var host = Environment.GetEnvironmentVariable("SONAR_HOST")
        ?? throw new InvalidOperationException("SONAR_HOST environment variable is not set.");
    var token = Environment.GetEnvironmentVariable("SONAR_TOKEN")
        ?? throw new InvalidOperationException("SONAR_TOKEN environment variable must be set.");

    // Create an HttpClient with the base address set to the JIRA host
    var client = new HttpClient() { BaseAddress = new Uri(host) };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("itbusina/sonar-mcp", "1.0"));
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    // Add Bearer Authentication using environment variable JIRA_PAT
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    return client;
});

var app = builder.Build();

await app.RunAsync();