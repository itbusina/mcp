namespace Models
{
    public class ProjectsSearchResponse
    {
        public Paging? Paging { get; set; }

        public IEnumerable<Component>? Components { get; set; }
    }
}
