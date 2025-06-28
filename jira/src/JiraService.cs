using Microsoft.Extensions.Logging;

namespace jira
{
    public class JiraService(HttpClient httpClient, ILogger<JiraService> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<JiraService> _logger = logger;

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

            var endpoint = $"/rest/api/3/search?{string.Join("&", queryParams)}";

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
