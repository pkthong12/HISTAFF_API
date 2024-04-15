namespace API.DTO
{
    public class HuAllowanceEmpDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? AllowanceId { get; set; }
        public decimal? Monney { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Extra
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PosName { get; set; }
        public string? AllowanceName { get; set; }
        public decimal? Coefficient { get; set; }
        public decimal? CoefficientAllwance { get; set; }
        public long? StatusId { get; set; } // Trạng thái
        public long? WorkStatusId { get; set; }
        public int? JobOrderNum { get; set; }


    }
}
