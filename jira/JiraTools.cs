using ModelContextProtocol.Server;
using System.ComponentModel;

namespace jira;

[McpServerToolType]
public static class JiraTools
{
    [McpServerTool, Description("Jira Search.")]
    public static async Task<string> Search(
        HttpClient client,
        [Description("The Jira query language")] string jql)
    {
        // Construct the Jira search API endpoint for REST API v3
        var endpoint = $"/rest/api/3/search?jql={Uri.EscapeDataString(jql)}";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
}