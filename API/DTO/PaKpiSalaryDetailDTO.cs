namespace API.DTO
{
    public class PaKpiSalaryDetailDTO
    {
        public long? Id { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public long? KpiTargetId { get; set; }
        public decimal? RealValue { get; set; }
        public decimal? StartValue { get; set; }
        public decimal? EqualValue { get; set; }
        public decimal? KpiSalary { get; set; }

        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsPaySalary { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
