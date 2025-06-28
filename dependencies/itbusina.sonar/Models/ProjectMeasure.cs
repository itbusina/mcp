using System.Text.Json.Serialization;
using itbusina.sonar.Enums;

namespace itbusina.sonar.Models
{
    public class ProjectMeasure
    {
        [JsonPropertyName(name: "Quality Gate Status")]
        public string? QualityGateStatus { get; internal set; }

        [JsonPropertyName(name: "Quality Gate Details")]
        public string? QualityGateDetails { get; internal set; }

        [JsonPropertyName(name: "Bugs")]
        public int? NumberOfBugs { get; set; }

        /*
         *  A = 0 Bugs
            B = at least 1 Minor Bug
            C = at least 1 Major Bug
            D = at least 1 Critical Bug
            E = at least 1 Blocker Bug
         */
        [JsonPropertyName(name: "Reliability Rating")]
        public Rating? ReliabilityRating { get; internal set; }

        [JsonPropertyName(name: "Vulnerabilities")]
        public int? NumberOfVulnerabilities { get; set; }

        /*
         *  A = 0 Vulnerabilities
            B = at least 1 Minor Vulnerability
            C = at least 1 Major Vulnerability
            D = at least 1 Critical Vulnerability
            E = at least 1 Blocker Vulnerability
         */
        [JsonPropertyName(name: "Security Rating")]
        public Rating? SecurityRating { get; set; }

        /*
         * (Formerly the SQALE rating.) Rating given to your project related to the value of your Technical Debt Ratio. The default Maintainability Rating grid is:

            A=0-0.05, B=0.06-0.1, C=0.11-0.20, D=0.21-0.5, E=0.51-1

            The Maintainability Rating scale can be alternately stated by saying that if the outstanding remediation cost is:

            <=5% of the time that has already gone into the application, the rating is A
            between 6 to 10% the rating is a B
            between 11 to 20% the rating is a C
            between 21 to 50% the rating is a D
            anything over 50% is an E
         */
        [JsonPropertyName(name: "Maintainability Rating")]
        public Rating? MaintainabilityRating { get; internal set; }

        [JsonPropertyName(name: "Code Coverage (%)")]
        public string? PercentageOfCodeCoverage { get; set; }

        [JsonPropertyName(name: "# Unit Tests")]
        public int? NumberOfUnitTests { get; set; }

        [JsonPropertyName(name: "Unit Tests Duration")]
        public int? TestExecutionTime { get; set; }

        [JsonPropertyName(name: "Debt (days)")]
        public int? DebtInDays { get; set; }

        [JsonPropertyName(name: "Debt (minutes)")]
        public int? DebtInMinutes { get; set; }

        [JsonPropertyName(name: "# Code Smells")]
        public int? NumberOfCodeSmells { get; set; }

        [JsonPropertyName(name: "Code Duplication (%)")]
        public string? PercentageOfCodeDuplication { get; set; }

        [JsonPropertyName(name: "# Duplicated Blocks")]
        public int? NumberOfDuplicatedBlocks { get; set; }

        [JsonPropertyName(name: "LoC")]
        public int? NumberOfLinesOfCode { get; set; }

        //[JsonProperty(PropertyName = "# Security Hotspots")]
        //public int? NumberOfSecurityHotspots { get; set; }

        /*
         * The Security Review Rating is a letter grade based on the percentage of Reviewed (Fixed or Safe) Security Hotspots.
            A = >= 80%
            B = >= 70% and <80%
            C = >= 50% and <70%
            D = >= 30% and <50%
            E = < 30%
         */
        [JsonPropertyName(name: "Security Review Rating")]
        public Rating? SecurityReviewRating { get; set; }

        [JsonPropertyName(name: "Reliability Remediation Rating (minutes)")]
        public int ReliabilityRemediationRatingInMinutes { get; set; }

        [JsonPropertyName(name: "Reliability Remediation Rating (days)")]
        public int ReliabilityRemediationRatingInDays { get; set; }

        [JsonPropertyName(name: "Security Remediation Effort (minutes)")]
        public int? SecurityRemediationEffortInMinutes { get; set; }

        [JsonPropertyName(name: "Security Remediation Effort (days)")]
        public int? SecurityRemediationEffortInDays { get; set; }

        [JsonPropertyName(name: "Cyclomatic Complexity")]
        public int? CyclomaticComplexity { get; set; }

        [JsonPropertyName(name: "Cognitive Complexity")]
        public int? CognitiveComplexity { get; set; }
    }
}