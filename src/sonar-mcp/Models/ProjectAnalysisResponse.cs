namespace Models
{
    public class ProjectAnalysisResponse
    {
        public Paging? Paging { get; set; }

        public IEnumerable<ProjectAnalysis>? Analyses { get; set; }
    }
}