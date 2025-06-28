using itbusina.sonar.Enums;
using itbusina.sonar.Models;
using itbusina.sonar.RestClients;

namespace itbusina.sonar
{
    public class SonarService(SonarApiClient client)
    {
        public async Task<FavoriteProjectsSearchResponse?> GetFavorites(int page = 1, int pageSize = 100)
        {
            return await client.GetFavorites(page, pageSize);
        }

        public async Task<MeasureComponentResponse?> GetMeasure(Project project)
        {
            return await client.GetMeasure(project.Name);
        }

        public async Task<IEnumerable<MeasureComponentResponse?>> GetMeasures(IEnumerable<Project> projects)
        {
            return await Task.WhenAll(projects
                .AsParallel()
                .Select(async project => await GetMeasure(project)));
        }

        public async Task<ProjectStatus?> GetProjectStatus(Project project)
        {
            var response = await client.GetProjectStatus(project.Name);
            return new ProjectStatus
            {
                Name = project.Name,
                Status = response?.ProjectStatus?.Status
            };
        }

        public async Task<IEnumerable<ProjectStatus?>> GetProjectStatuses(IEnumerable<Project> projects)
        {
            return await Task.WhenAll(projects
                .AsParallel()
                .Select(async project => await GetProjectStatus(project)));
        }

        public async Task<SonarStatus?> GetSonarStatus()
        {
            return await client.GetStatus();
        }

        public async Task<IEnumerable<ProjectDetails?>> GetProjectsDetails(IEnumerable<Project> projects)
        {
            return await Task.WhenAll(projects
                .AsParallel()
                .Select(async project =>
                {
                    try
                    {
                        return await GetProjectDetails(project);
                    }
                    catch
                    {
                        return new ProjectDetails
                        {
                            Name = project.Name,
                            Organization = project.Organization,
                            QualityGateName = "Undefined",
                            LastAnalysis = "Undefined"
                        };
                    }
                }));
        }

        public async Task<IEnumerable<GetProjectHistory?>> GetProjectsHistory(IEnumerable<Project> projects)
        {
            return await Task.WhenAll(projects
                .AsParallel()
                .Select(async project => await GetProjectHistory(project)));
        }

        private async Task<GetProjectHistory?> GetProjectHistory(Project project)
        {
            var history = await client.GetHistory(project.Name, 1);
            return new GetProjectHistory
            {
                Name = project.Name,
                History = history
            };
        }

        private async Task<ProjectDetails?> GetProjectDetails(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
                return default;

            // get project measures
            var responseJson = await client.GetMeasure(project.Name);
            if (responseJson?.Component?.Measures == null)
                return default;

            // get project quality gate
            var qualityGate = await client.GetQualityGateByProject(project.Organization, project.Name);
            if (qualityGate == null)
                return default;

            // get project last analysis
            var projectAnalyses = await client.GetProjectAnalyses(project.Name);
            if (projectAnalyses == null)
                return default;

            var projectDetails = new ProjectDetails
            {
                Name = project.Name,
                Organization = project.Organization,
                QualityGateId = qualityGate.QualityGate.Id,
                QualityGateName = qualityGate.QualityGate.Name,
                LastAnalysis = projectAnalyses.Analyses?.FirstOrDefault()?.Date ?? "-"
            };

            foreach (var measure in responseJson?.Component?.Measures)
            {
                if (measure == null) continue;

                switch (measure.Metric)
                {
                    case "tests": { projectDetails.NumberOfUnitTests = int.Parse(measure.Value); break; }
                    case "test_execution_time": { projectDetails.TestExecutionTime = int.Parse(measure.Value); break; }
                    case "coverage": { projectDetails.PercentageOfCodeCoverage = measure.Value; break; }
                    case "complexity": { projectDetails.CyclomaticComplexity = int.Parse(measure.Value); break; }
                    case "cognitive_complexity": projectDetails.CognitiveComplexity = int.Parse(measure.Value); break;
                    case "bugs": { projectDetails.NumberOfBugs = int.Parse(measure.Value); break; }
                    case "vulnerabilities": { projectDetails.NumberOfVulnerabilities = int.Parse(measure.Value); break; }
                    case "security_rating": { projectDetails.SecurityRating = (Rating)Enum.Parse(typeof(Rating), Math.Abs(double.Parse(measure.Value)).ToString()); break; }
                    //case "security_hotspots": { projectSummary.NumberOfSecurityHotspots = int.Parse(measure.Value); break; }
                    case "security_review_rating": { projectDetails.SecurityReviewRating = (Rating)Enum.Parse(typeof(Rating), Math.Abs(double.Parse(measure.Value)).ToString()); break; }
                    case "security_remediation_effort":
                        {
                            projectDetails.SecurityRemediationEffortInMinutes = int.Parse(measure.Value);
                            projectDetails.SecurityRemediationEffortInDays = int.Parse(measure.Value) / (60 * 8); // 60 minutes in an hour. 8 hours in working day
                            break;
                        }
                    case "code_smells": { projectDetails.NumberOfCodeSmells = int.Parse(measure.Value); break; }
                    case "duplicated_lines_density": { projectDetails.PercentageOfCodeDuplication = measure.Value; break; }
                    case "duplicated_blocks": { projectDetails.NumberOfDuplicatedBlocks = int.Parse(measure.Value); break; }
                    case "ncloc": { projectDetails.NumberOfLinesOfCode = int.Parse(measure.Value); break; }
                    case "sqale_index":
                        {
                            projectDetails.DebtInMinutes = int.Parse(measure.Value);
                            projectDetails.DebtInDays = int.Parse(measure.Value) / (60 * 8); // 60 minutes in an hour. 8 hours in working day
                            break;
                        }
                    case "sqale_rating": { projectDetails.MaintainabilityRating = (Rating)Enum.Parse(typeof(Rating), Math.Abs(double.Parse(measure.Value)).ToString()); break; }
                    case "alert_status": { projectDetails.QualityGateStatus = measure.Value; break; }
                    case "quality_gate_details": { projectDetails.QualityGateDetails = measure.Value; break; }
                    case "reliability_rating": { projectDetails.ReliabilityRating = (Rating)Enum.Parse(typeof(Rating), Math.Abs(double.Parse(measure.Value)).ToString()); break; }
                    case "reliability_remediation_effort":
                        {
                            projectDetails.ReliabilityRemediationRatingInMinutes = int.Parse(measure.Value);
                            projectDetails.ReliabilityRemediationRatingInDays = int.Parse(measure.Value) / (60 * 8); // 60 minutes in an hour. 8 hours in working day
                            break;
                        }
                    //case "projects": { projectSummary.NumberOfProjects = int.Parse(measure.Value); break; }
                    default: break;
                }
            }

            return projectDetails;
        }
    }
}