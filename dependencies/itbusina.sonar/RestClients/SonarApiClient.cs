using itbusina.sonar.Constants;
using itbusina.sonar.Models;

namespace itbusina.sonar.RestClients
{
    public class SonarApiClient(AuthenticatedClient client)
    {
        public async Task<MeasureComponentResponse?> GetMeasure(string projectName)
        {
            var url = string.Format(SonarConstants.MeasuresApiTemplate, projectName) + string.Format(SonarConstants.MetricKeysTemplate, string.Join(SonarConstants.MetricsSeparator, SonarConstants.SonarMetrics));
            return await client.GetJson<MeasureComponentResponse>(url);
        }

        public async Task<MeasureSearchHistoryResponse?> GetHistory(string projectName, int page)
        {
            var url = string.Format(SonarConstants.HistoriesApiTemplate, projectName, string.Join(SonarConstants.MetricsSeparator, SonarConstants.SonarMetrics), SonarConstants.MaxPageSize, page);
            return await client.GetJson<MeasureSearchHistoryResponse>(url);
        }

        public async Task<SonarStatus?> GetStatus()
        {
            return await client.GetJson<SonarStatus>("api/system/status");
        }

        public async Task<FavoriteProjectsSearchResponse?> GetFavorites(int page = 1, int pageSize = 100)
        {
            return await client.GetJson<FavoriteProjectsSearchResponse>($"api/favorites/search?p={page}&ps={pageSize}");
        }

        public async Task<ProjectsSearchResponse?> GetProjects(string query = "", int page = 1, int pageSize = 100)
        {
            return await client.GetJson<ProjectsSearchResponse>($"api/projects/search?q={query}&p={page}&ps={pageSize}");
        }

        public async Task<ProjectStatusResponse?> GetProjectStatus(string projectName)
        {
            return await client.GetJson<ProjectStatusResponse>($"api/qualitygates/project_status?projectKey={projectName}");
        }

        public async Task<QualityGateResponse?> GetQualityGateByProject(string organization, string projectName)
        {
            return await client.GetJson<QualityGateResponse>($"api/qualitygates/get_by_project?organization={organization}&project={projectName}");
        }

        public async Task<ProjectAnalysisResponse?> GetProjectAnalyses(string projectName)
        {
            return await client.GetJson<ProjectAnalysisResponse>($"api/project_analyses/search?project={projectName}");
        }
    }
}