namespace Models
{
    public class QualityGate
    {
        public int? Id { get; set; }

        public required string Name { get; set; }

        public bool Default { get; set; }
    }
}
