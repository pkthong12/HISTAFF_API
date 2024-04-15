namespace API.DTO
{
    public class SyAuditlogDTO
    {
        public long? AuditlogId { get; set; }
        public string? UserId { get; set; }
        public DateTime? EventDate { get; set; }
        public string? EventType { get; set; }
        public string? TableName { get; set; }
        public string? RecordId { get; set; }
        public string? ColumnName { get; set; }
        public string? OriginalValue { get; set; }
        public string? NewValue { get; set; }
    }
}
