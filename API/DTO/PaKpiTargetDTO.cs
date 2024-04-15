namespace API.DTO
{
    public class PaKpiTargetDTO
    {
        public long? Id { get; set; }
        public long? KpiGroupId { get; set; }
        public int? ColId { get; set; }
        public long? TypeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public string? ColName { get; set; }
        public int? MaxValue { get; set; }

        public bool? IsRealValue { get; set; }
        public bool? IsPaySalary { get; set; }
        public bool? IsImportKpi { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystem { get; set; }

        public int? Orders { get; set; }

        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
