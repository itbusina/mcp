
using itbusina.sonar;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace sonar
{
    [McpServerToolType, Description("Tools to access SonarCloud and SonarQube REST API.")]
    public static class Tools
    {
        [McpServerTool, Description("Search for the authenticated user favorites. Requires authentication.")]
        public static async Task<string> GetFavorites(
            SonarClient client,
            ILogger<Tool> logger,
            [Description("1-based page number. Default value: 1. Example value: 42. Optional.")] int? page = 1,
            [Description("Page size. Must be greater than 0 and less or equal than 500. Default: 100. Maximum: 500. Example: 20. Optional.")] int? pageSize = 100)
        {
            page ??= 1;
            pageSize ??= 100;

            var favorites = await client.GetFavoritesAsync(page, pageSize);
            return JsonSerializer.Serialize(favorites);
        }

        [McpServerTool, Description("Get the quality gate of a project.")]
        public static async Task<string> GetProjectQualityGate(
            SonarClient client,
            ILogger<Tool> logger,
            [Description("Organization key. Required.")] string organization,
            [Description("Project key. Required.")] string project)
        {
            var response = await client.GetQualityGateByProjectAsync(organization, project);
            return JsonSerializer.Serialize(response);
        }

        [McpServerTool, Description(@"Get the quality gate status of a project or a Compute Engine task.
            Either 'analysisId', 'projectId' or 'projectKey' must be provided
            The different statuses returned are: OK, WARN, ERROR, NONE. The NONE status is returned when there is no quality gate associated with the analysis.")]
        public static async Task<string> GetProjectQualityGateStatus(
            SonarClient client,
            ILogger<Tool> logger,
            [Description("Analysis id. Optional. Example: 'AU-TpxcA-iU5OvuD2FL1'")] string? analysisId = null,
            [Description("Branch key. Optional. Example: 'feature/my_branch'")] string? branch = null,
            [Description("Project id. Optional. Doesn't work with branches or pull requests. Example: 'AU-Tpxb--iU5OvuD2FLy'")] string? projectId = null,
            [Description("Project key. Optional. Example: 'my_project'")] string? projectKey = null,
            [Description("Pull request id. Optional. Example: '5461'")] string? pullRequest = null
            )
        {
            var response = await client.GetProjectQualityGateStatusAsync(projectKey, analysisId, branch, projectId, pullRequest);
            return JsonSerializer.Serialize(response);
        }

        [McpServerTool, Description("Return component with specified measures. The componentId or the component parameter must be provided.")]
        public static async Task<string> GetMeasures(
            SonarClient client,
            ILogger<Tool> logger,
            [Description("The component name. Required.")] string component)
        {
            var response = await client.GetMeasureAsync(component);
            return JsonSerializer.Serialize(response);
        }
    }
}