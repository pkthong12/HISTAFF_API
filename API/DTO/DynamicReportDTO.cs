namespace API.DTO
{
    public class DynamicReportDTO
    {
        public long? Id { get; set; }
        public string[]? WorkStatus { get; set; }
        public long[]? OrgIds { get; set; }
        public string? QueryForm { get; set; }
        public string? ViewName { get; set; }
        public string? ColArrayChanged { get; set; }
        public string? ReportNameToSave { get; set; }
        public string? PrefixTrans { get; set; }
        public List<REPORT_DATA_STAFF_PROFILE>? ReportStaffProfile { get; set; }
    }
}
