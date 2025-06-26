using ModelContextProtocol.Server;
using System.ComponentModel;

namespace jira;

[McpServerToolType]
public static class JiraTools
{
    [McpServerTool, Description("Jira Search.")]
    public static async Task<string> Search(
        HttpClient client,
        [Description("The Jira query language")] string jql,
        [Description("The index of the first issue to return (pagination). Optional.")] int? startAt = 0,
        [Description("The maximum number of issues to return (pagination). Optional. Min 1, Max 1000. Default 10.")] int? maxResults = 10,
        [Description("Comma-separated list of fields to include in the response. Optional.")] string? fields = null,
        [Description("Comma-separated list of entities to expand. Optional.")] string? expand = null)
    {
        if (startAt.HasValue && startAt.Value < 0)
            throw new ArgumentOutOfRangeException(nameof(startAt), "startAt must be non-negative.");
        if (maxResults.HasValue && (maxResults.Value < 1 || maxResults.Value > 1000))
            throw new ArgumentOutOfRangeException(nameof(maxResults), "maxResults must be between 1 and 1000.");

        var queryParams = new List<string> { $"jql={Uri.EscapeDataString(jql)}" };
        if (startAt.HasValue)
            queryParams.Add($"startAt={startAt.Value}");
        if (maxResults.HasValue)
            queryParams.Add($"maxResults={maxResults.Value}");
        if (!string.IsNullOrWhiteSpace(fields))
            queryParams.Add($"fields={Uri.EscapeDataString(fields)}");
        if (!string.IsNullOrWhiteSpace(expand))
            queryParams.Add($"expand={Uri.EscapeDataString(expand)}");

        var endpoint = $"/rest/api/3/search?{string.Join("&", queryParams)}";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
}