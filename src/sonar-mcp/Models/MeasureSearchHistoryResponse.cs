namespace Models
{
    public class MeasureSearchHistoryResponse
    {
        public List<HistoryMeasure>? Measures { get; set; }

        public Paging? Paging { get; set; }
    }
}