namespace API.DTO
{
    public class HuDynamicReportDtlDTO
    {
        public long? Id { get; set; }
        public long? ViewId { get; set; }
        public string? ColumnType { get; set; }
        public string? ColumnName { get; set; }
        public string? Translate { get; set; }
        public int? ColumnOrder { get; set; }
    }
}
