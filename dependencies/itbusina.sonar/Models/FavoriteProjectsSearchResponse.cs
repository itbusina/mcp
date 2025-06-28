namespace itbusina.sonar.Models
{
    public class FavoriteProjectsSearchResponse
    {
        public Paging? Paging { get; set; }

        public IEnumerable<Favorite>? Favorites { get; set; }
    }
}
