namespace itbusina.sonar.Models
{
    public class Component
    {
        public string? Id { get; set; }

        public string? Key { get; set; }

        public string? Name { get; set; }

        public string? Qualifier { get; set; }

        public List<Measure>? Measures { get; set; }
    }
}