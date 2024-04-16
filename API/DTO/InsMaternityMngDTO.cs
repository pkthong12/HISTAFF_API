using API.Main;

namespace API.DTO
{
    public class InsMaternityMngDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PosName { get; set; }
        public string? InsuranceNo { get; set; }
        public DateTime? NgayDuSinh { get; set; }
        public bool? IsNghiThaiSan { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? SoCon { get; set; }
        public decimal? TienTamUng { get; set; }
        public string? Remark { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? FromDateEnjoy { get; set; }
        public DateTime? ToDateEnjoy { get; set; }
        public DateTime? NgayDiLamSom { get; set; }
        public int? JobOrderNum { get; set; }
        public string? Note { get; set; }

    }
}
