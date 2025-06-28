using System.Net.Http.Headers;
using System.Net.Http.Json;
using itbusina.sonar.Constants;
using itbusina.sonar.Models;

namespace itbusina.sonar
{
    public class SonarClient
    {
        private readonly HttpClient _httpClient;

        public SonarClient(string host, string? token)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(host)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<MeasureComponentResponse?> GetMeasureAsync(string projectName)
        {
            var url = string.Format(SonarConstants.MeasuresApiTemplate, projectName) + string.Format(SonarConstants.MetricKeysTemplate, string.Join(SonarConstants.MetricsSeparator, SonarConstants.SonarMetrics));
            return await _httpClient.GetFromJsonAsync<MeasureComponentResponse>(url);
        }

        public async Task<MeasureSearchHistoryResponse?> GetHistoryAsync(string projectName, int page)
        {
            var url = string.Format(SonarConstants.HistoriesApiTemplate, projectName, string.Join(SonarConstants.MetricsSeparator, SonarConstants.SonarMetrics), SonarConstants.MaxPageSize, page);
            return await _httpClient.GetFromJsonAsync<MeasureSearchHistoryResponse>(url);
        }

        public async Task<SonarStatus?> GetStatusAsync()
        {
            return await _httpClient.GetFromJsonAsync<SonarStatus>("api/system/status");
        }

        public async Task<FavoriteProjectsSearchResponse?> GetFavoritesAsync(int? page = 1, int? pageSize = 100)
        {
            return await _httpClient.GetFromJsonAsync<FavoriteProjectsSearchResponse>($"api/favorites/search?p={page}&ps={pageSize}");
        }

        public async Task<ProjectsSearchResponse?> GetProjectsAsync(string query = "", int page = 1, int pageSize = 100)
        {
            return await _httpClient.GetFromJsonAsync<ProjectsSearchResponse>($"api/projects/search?q={query}&p={page}&ps={pageSize}");
        }

        public async Task<ProjectStatusResponse?> GetProjectQualityGateStatusAsync(string? projectKey = null, string? analysisId= null, string? branch= null, string? projectId= null, string? pullRequest= null)
        {
            var url = $"api/qualitygates/project_status?projectKey={projectKey}";
            if (!string.IsNullOrEmpty(analysisId))
                url += $"&analysisId={analysisId}";
            if (!string.IsNullOrEmpty(branch))
                url += $"&branch={branch}";
            if (!string.IsNullOrEmpty(projectId))
                url += $"&projectId={projectId}";
            if (!string.IsNullOrEmpty(pullRequest))
                url += $"&pullRequest={pullRequest}";

            return await _httpClient.GetFromJsonAsync<ProjectStatusResponse>(url);
        }

        public async Task<QualityGateResponse?> GetQualityGateByProjectAsync(string organization, string projectName)
        {
            return await _httpClient.GetFromJsonAsync<QualityGateResponse>($"api/qualitygates/get_by_project?organization={organization}&project={projectName}");
        }

        public async Task<ProjectAnalysisResponse?> GetProjectAnalysesAsync(string projectName)
        {
            return await _httpClient.GetFromJsonAsync<ProjectAnalysisResponse>($"api/project_analyses/search?project={projectName}");
        }
    }
}