using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class AllowanceEmpDTO : Pagings
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PosName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? AllowanceId { get; set; }
        public string? AllowanceName { get; set; }
        public decimal? Monney { get; set; }
        public decimal? Coefficient { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }

    public class AllowanceEmpInputDTO
    {
        public long? Id { get; set; }
        public long? AllowanceId { get; set; }
        public decimal? Monney { get; set; }
        public decimal? Coefficient { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? Note { get; set; }
        //public List<AllowanceListEmp> Emps { get; set; }
        public long[]? EmployeeIds { get; set; }
    }
    public class AllowanceListEmp
    {
        public long EmployeeId { get; set; }
        public string PosName { get; set; }
        public string OrgName { get; set; }
    }

}
