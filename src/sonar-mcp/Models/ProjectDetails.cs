namespace Models
{
    public class ProjectDetails : ProjectMeasure
    {
        public required string Name { get; set; }

        public required string Organization { get; set; }

        public int? QualityGateId { get; set; }

        public required string QualityGateName { get; set; }

        public required string LastAnalysis { get; set; }
    }
}