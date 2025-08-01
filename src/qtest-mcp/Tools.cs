using ModelContextProtocol.Server;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace qtest_mcp;

[McpServerToolType]
public static class Tools
{
    [McpServerTool, Description("Returns a list of qTest projects.")]
    public static async Task<string> GetProjects(
        HttpClient client)
    {
        var endpoint = $"/api/v3/projects";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }

    [McpServerTool, Description("Returns a list of test cases by qTest project id.")]
    public static async Task<string> GetTestCasesByProjectId(
        HttpClient client,
        [Description("The qTest project ID."), Required] string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
            throw new ArgumentException("Project ID must be provided.", nameof(projectId));

        var endpoint = $"/api/v3/projects/{projectId}/test-cases";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }

    [McpServerTool, Description("This API mimics the Data Query function of qTest Manager web app. It provides the capability to query Requirements, Test Cases, Test Runs and internal Defects.")]
    public static async Task<string> SearchProjectTestCases(
        HttpClient client,
        [Description("The qTest project ID."), Required] string projectId,
        [Description("The object_type (required): Its value can be releases, requirements, test-cases, test-runs, test-suites, test-cycles, test-logs, builds, or defects."), Required] string object_type,
        [Description("Specify which object fields you want to include in the response. If you omit it or specify an asterisk (*), all fields are included, in that case the fields = [\"*\"]. Fields property is an array of string values. Default fields are id and name."), Required] List<string> fields,
        [Description("qTest Query Language. Specify a structured query to search for qTest Manager objects. Basically, you can use the Query Summary text as in qTest web app for this attribute. Example: \"'name' ~ 'LS-1626'\""), Required] string query,
        [Description("The page number. Optional. Default 1.")] string page,
        [Description("The number of items to return. Optional. Default 50.")] string page_size)
    {
        if (string.IsNullOrWhiteSpace(projectId))
            throw new ArgumentException("Project ID must be provided.", nameof(projectId));
        if (string.IsNullOrWhiteSpace(object_type))
            throw new ArgumentException("Object type must be provided.", nameof(object_type));
        if (fields == null || fields.Count == 0)
            throw new ArgumentException("Fields must be provided.", nameof(fields));
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query must be provided.", nameof(query));

        var endpoint = $"/api/v3/projects/{projectId}/search";
        var payload = new
        {
            object_type,
            fields,
            query,
            page,
            page_size
        };
        var jsonContent = JsonSerializer.Serialize(payload);
        var response = await client.PostAsync(endpoint, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }

    [McpServerTool, Description("Returns all Test Steps of a Test Case.")]
    public static async Task<string> GetTestStepsByTestCaseId(
        HttpClient client,
        [Description("The qTest project ID."), Required] string projectId,
        [Description("The qTest test case ID."), Required] string testCaseId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
            throw new ArgumentException("Project ID must be provided.", nameof(projectId));
        if (string.IsNullOrWhiteSpace(testCaseId))
            throw new ArgumentException("Test Case ID must be provided.", nameof(testCaseId));

        var endpoint = $"/api/v3/projects/{projectId}/test-cases/{testCaseId}/test-steps";
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
}