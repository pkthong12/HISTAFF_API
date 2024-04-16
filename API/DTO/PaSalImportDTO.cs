using API.Main;

namespace API.DTO
{
    public class PaSalImportDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public string? Note { get; set; }
        public string? ObjSalaryName { get; set; }
        public long? ObjSalaryId { get; set; }
        public long? Year { get; set; }
        public long? OrgId { get; set; }
        public long? ElementId { get; set; }
        public long? PeriodId { get; set; }
        public double? Cl11 { get; set; }
        public double? Cl29 { get; set; }
        public double? Cl31 { get; set; }
        public double? Cl33 { get; set; }
        public double? Clchinh8 { get; set; }
        public double? Csum1 { get; set; }
        public double? Csum2 { get; set; }
        public double? Deduct3 { get; set; }
        public double? Tax54 { get; set; }
        public int? TypeSal { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
