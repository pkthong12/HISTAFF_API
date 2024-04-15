using API.All.HRM.DynamicReport;

namespace API.DTO
{
    public class HuDynamicReportDtlTotalDTO
    {
        public long? Id { get; set; }
        public string? Json { get; set; }
        public string? Expression { get; set; } 
        public string? ReportName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //public int? CONDITION_FILTER_DETAIL { get; set; }
        //public EnumConditionFilterDetail? ConditionFilterDetail { get; set; }
    }

    
}
