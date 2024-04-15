namespace API.DTO
{
    public class AtSalaryPeriodDtlDTO
    {
        public long? Id { get; set; }
        public long? PeriodId { get; set; }
        public long? OrgId { get; set; }
        public long? EmpId { get; set; }

        public decimal? WorkingStandard { get; set; }
        public decimal? StandardTime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }








    }
}
