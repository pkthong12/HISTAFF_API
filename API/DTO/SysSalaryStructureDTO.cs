namespace API.DTO
{
    public class SysSalaryStructureDTO
    {
        public long? Id { get; set; }
        public long? AreaId { get; set; }
        public long? SalaryTypeId { get; set; }
        public long? ElementId { get; set; }

        public bool? IsVisible { get; set; }
        public bool? IsCalculate { get; set; }
        public bool? IsImport { get; set; }

        public int? Orders { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
