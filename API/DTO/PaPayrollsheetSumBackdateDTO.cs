﻿using API.Main;

namespace API.DTO
{
    public class PaPayrollsheetSumBackdateDTO : BaseDTO
    {
        public long? PeriodId { get; set; }
        public long? PeriodAddId { get; set; }
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
        public long[]? OrgIds { get; set; }
        public int? Year { get; set; }
        public long? PayrollFund { get; set; }
        public string? EmployeeCal { get; set; }
        public DateTime? PayDate { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? DepartmentName { get; set; }
        public string? JoinDateSearch { get; set; }
        public string? PayDateSearch { get; set; }
        public string? FundName { get; set; }
        // USING TO PAGINATION
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        // USING TO CHANGE STATUS PAROX
        public int? StatusParoxBackdate { get; set; }
    }
}
