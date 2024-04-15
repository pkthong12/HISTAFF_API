namespace API.DTO
{
    public class PaKpiSalaryDetailTmpDTO
    {
        public long? Id { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public long? KpiTargetId { get; set; }
        public decimal? RealValue { get; set; }
        public decimal? StartValue { get; set; }
        public decimal? EqualValue { get; set; }
    }
}
