using API.Main;

namespace API.DTO
{
    public class PaPayrollTaxYearDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public long? TitleId { get; set; }
        public long? ObjSalaryId { get; set; }
        public int? WorkingId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? JoinDate { get; set; }
        public long? JobpositionId { get; set; }
        // USING TO EXEC STORE
        public long?[]? OrgIds { get; set; }
        public int? Year { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? EmployeeCal { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? DepartmentName { get; set; }
        public string? TaxCodeSearch { get; set; }
        public string? IdNoSearch { get; set; }

        // USING TO PAGINATION
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        // USING TO CHANGE STATUS PAROX
        public int? Lock { get; set; }
    }
}
