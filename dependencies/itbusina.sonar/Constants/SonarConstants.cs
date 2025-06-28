namespace itbusina.sonar.Constants
{
    public class SonarConstants
    {
        // for metrics details go her: https://docs.sonarqube.org/latest/user-guide/metric-definitions/
        public const string MetricsSeparator = "%2C";

        public const string HistoriesApiTemplate = "/api/measures/search_history?component={0}&metrics={1}&ps={2}&p={3}";

        public const int MaxPageSize = 1000;

        public static readonly List<string> SonarMetrics =
        [
            "alert_status",
            "quality_gate_details",
            "bugs",
            "complexity", // Cyclomatic complexity
            "cognitive_complexity", // Cognitive Complexity 
            "reliability_rating",
            "reliability_remediation_effort",
            "vulnerabilities",
            "code_smells",
            "sqale_rating", // technical debt rating
            "sqale_index",
            "coverage",
            "tests",
            "test_execution_time",
            "duplicated_lines_density",
            "duplicated_blocks",
            "ncloc",
            "ncloc_language_distribution",
            "projects",
            "security_rating",
            "security_remediation_effort",
            "security_hotspots",
            "security_review_rating",
            "security_hotspots_reviewed",
            "quality_gate_details",
            // "new_vulnerabilities",
            // "new_security_remediation_effort",
            // "new_security_hotspots",
            // "new_security_review_rating",
            // "new_bugs",
            // "new_reliability_rating",
            // "new_vulnerabilities",
            // "new_security_rating",
            // "new_code_smells",
            // "new_maintainability_rating",
            // "new_technical_debt",
            // "new_coverage",
            // "new_lines_to_cover",
            // "new_duplicated_lines_density",
            // "new_lines",
        ];
    }
}