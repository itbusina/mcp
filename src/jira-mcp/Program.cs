using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton(_ =>
{
    var auth = Environment.GetEnvironmentVariable("JIRA_AUTH_TYPE") ?? "bearer";
    var user = Environment.GetEnvironmentVariable("JIRA_USER");
    var host = Environment.GetEnvironmentVariable("JIRA_HOST")
        ?? throw new InvalidOperationException("JIRA_HOST environment variable is not set.");
    var token = Environment.GetEnvironmentVariable("JIRA_TOKEN")
        ?? throw new InvalidOperationException("JIRA_TOKEN environment variable must be set.");

    // Create an HttpClient with the base address set to the JIRA host
    var client = new HttpClient() { BaseAddress = new Uri(host) };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("itbusina-jira-mcp", "1.0"));
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    client.DefaultRequestHeaders.Authorization = (auth?.ToLower() ?? "bearer") switch
            {
                "basic" => string.IsNullOrEmpty(user)
                    ? throw new InvalidOperationException("JIRA_USER environment variable must be set for basic authentication.")
                    : new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{token}"))),
                "bearer" => new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token),
                _ => throw new InvalidOperationException($"Unknown JIRA_AUTH_TYPE: {auth}. Supported values are 'basic' or 'bearer'.")
            };

    return client;
});

var app = builder.Build();

await app.RunAsync();