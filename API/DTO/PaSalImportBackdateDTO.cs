using System.Drawing;
using API.Main;

namespace API.DTO
{
    // tạo lớp PaSalImportBackdateDTO
    // kế thừa lớp BaseDTO
    public class PaSalImportBackdateDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public string? ObjSalaryName { get; set; }
        public long? ObjSalaryId { get; set; }
        public long? Year { get; set; }
        public long? OrgId { get; set; }
        public long? ElementId { get; set; }
        public long? PeriodId { get; set; }
        public long? PeriodAddId { get; set; }
        public double? Clchinh8 { get; set; }
        public double? Deduct5 { get; set; }

        public double? Cl1 { get; set; }
        public double? Cl27 { get; set; }
        public int? TypeSal { get; set; }
        public string? Note { get; set; }
        public int? JobOrderNum { get; set; }
    }
}