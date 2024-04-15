using API.Main;

namespace API.DTO
{
    public class SysPaFormulaDTO : BaseDTO
    {
        public long? AreaId { get; set; }
        public long? SalaryTypeId { get; set; }
        public string? SalaryType { get; set; }
        public string? ColName { get; set; }
        public string? Formula { get; set; }
        public string? FormulaName { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? ObjSalary { get; set; }
    }
}
