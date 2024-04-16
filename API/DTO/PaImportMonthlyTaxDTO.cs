using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace API.DTO
{
    public class PaImportMonthlyTaxDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? Note { get; set; }
        public string? OrgName { get; set; }
        public DateTime? DateCalculate { get; set; }
        public string? ObjSalaryName { get; set; }
        public long? ObjSalaryId { get; set; }
        public long? DateCalculateId { get; set; }
        public long? Year { get; set; }
        public long? OrgId { get; set; }
        public long? ElementId { get; set; }
        public long? PeriodId { get; set; }
        public double? Clchinh8 { get; set; }
        public double? Tax20 { get; set; }
        public double? Tax21 { get; set; }
        public double? Tax23 { get; set; }
        public double? Tax24 { get; set; }
        public double? Tax25 { get; set; }
        public double? Tax26 { get; set; }
        public double? Tax27 { get; set; }
        public double? Tax28 { get; set; }
        public double? Tax29 { get; set; }
        public double? Tax30 { get; set; }
        public double? Tax31 { get; set; }
        public double? Tax32 { get; set; }
        public double? Tax33 { get; set; }
        public double? Tax34 { get; set; }
        public double? Tax35 { get; set; }
        public double? Tax36 { get; set; }
        public double? Tax37 { get; set; }
        public double? Tax38 { get; set; }
        public double? Tax41 { get; set; }
        public double? Tax42 { get; set; }
        public double? Tax43 { get; set; }
        public double? Tax44 { get; set; }
        public double? Deduct5 { get; set; }
        public int? TypeSal { get; set; }
        public int? JobOrderNum { get; set; }

    }
}
