using Microsoft.Extensions.Logging;

namespace itbusina.jira
{
    public class JiraService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JiraService> _logger;
        private readonly string _apiVersion;

        public JiraService(ILogger<JiraService> logger, string jiraHost, string? jiraUser, string jiraToken, string jiraAuthType, string apiVersion)
        {
            if (apiVersion != "2" && apiVersion != "3")
                throw new InvalidOperationException($"JIRA_API_VERSION must be '2' or '3'. Current value: '{apiVersion}'");

            _logger = logger;

            _apiVersion = apiVersion;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(jiraHost)
            };
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("jira-mcp", "1.0"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = (jiraAuthType?.ToLower() ?? "bearer") switch
            {
                "basic" => string.IsNullOrEmpty(jiraUser)
                    ? throw new InvalidOperationException("JIRA_USER environment variable must be set for basic authentication.")
                    : new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{jiraUser}:{jiraToken}"))),
                "bearer" => new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jiraToken),
                _ => throw new InvalidOperationException($"Unknown JIRA_AUTH_TYPE: {jiraAuthType}. Supported values are 'basic' or 'bearer'.")
            };
        }

        public async Task<string> SearchAsync(string jql, int? startAt = 0, int? maxResults = 10, string? fields = null, string? expand = null)
        {
            var queryParams = new List<string> { $"jql={Uri.EscapeDataString(jql)}" };
            if (startAt.HasValue)
                queryParams.Add($"startAt={startAt.Value}");
            if (maxResults.HasValue)
                queryParams.Add($"maxResults={maxResults.Value}");
            if (!string.IsNullOrWhiteSpace(fields))
                queryParams.Add($"fields={Uri.EscapeDataString(fields)}");
            if (!string.IsNullOrWhiteSpace(expand))
                queryParams.Add($"expand={Uri.EscapeDataString(expand)}");

            // Use the API version from the environment variable
            var endpoint = $"/rest/api/{_apiVersion}/search?{string.Join("&", queryParams)}";

            // Log the request
            _logger.LogInformation("Making GET request to Jira API: {Endpoint}", endpoint);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            // Log the response
            _logger.LogInformation("Received response from Jira API: {Json}", json);

            return json;
        }
    }
}
