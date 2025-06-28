using ModelContextProtocol.Server;
using System.ComponentModel;

namespace jira
{
    [McpServerToolType]
    public static class JiraTools
    {
        [McpServerTool, Description("Jira Search. Gets tickets (tasks, stories, defects, etc.) based on JQL (Jira Query Language).")]
        public static async Task<string> Search(
            JiraService jiraService,
            [Description("The Jira Query Language.")] string jql,
            [Description("The index of the first ticket to return (pagination). Optional.")] int? startAt = 0,
            [Description("The maximum number of tickets to return (pagination). Optional. Min 1, Max 1000. Default 10. When user asks for total number of tickets, this parameter is ignored.")] int? maxResults = 10,
            [Description("Comma-separated list of fields to include in the response. Optional.")] string? fields = null,
            [Description("Comma-separated list of entities to expand. Optional.")] string? expand = null)
        {
            if (startAt.HasValue && startAt.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(startAt), "startAt must be non-negative.");
            if (maxResults.HasValue && (maxResults.Value < 1 || maxResults.Value > 1000))
                throw new ArgumentOutOfRangeException(nameof(maxResults), "maxResults must be between 1 and 1000.");

            return await jiraService.SearchAsync(jql, startAt, maxResults, fields, expand);
        }
    }
}