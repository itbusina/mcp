namespace Models
{
    public class GetProjectHistory
    {
        public string? Name { get; set; }

        public MeasureSearchHistoryResponse? History {get; set;}
    }
}