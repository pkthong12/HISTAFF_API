using API.Main;

namespace API.DTO
{
    public class PaTaxAnnualImportDTO : BaseDTO
    {
        public long? Year { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? Note { get; set; }
        public string? OrgName { get; set; }
        public string? ObjSalaryName { get; set; }

        public long? OrgId { get; set; }
        public long? TitleId { get; set; }
        public long? ObjSalaryId { get; set; }
        public long? WorkingId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? JoinDate { get; set; }
        public long? JobpositionId { get; set; }
        public bool? IsCongdoan { get; set; }
        public bool? IsKhoan { get; set; }
        public double? LuongBhxh { get; set; }
        public double? LuongBhtn { get; set; }
        public double? LuongCoBan { get; set; }
        public double? Al2 { get; set; }
        public double? Amnl002 { get; set; }
        public string? Kc001 { get; set; }
        public double? Aaa { get; set; }
        public double? Pppp { get; set; }
        public double? Ttttt { get; set; }
        public double? Yyyyyy { get; set; }
        public double? Dmlk01 { get; set; }
        public double? A002 { get; set; }
        public double? Deduct5 { get; set; }
        public double? Clchinh3 { get; set; }
        public double? Clchinh4 { get; set; }
        public double? Cl1 { get; set; }
        public double? Cl27 { get; set; }
        public double? Tax18 { get; set; }
        public double? Tax26 { get; set; }
        public double? Tax23 { get; set; }
        public double? Tax44 { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
