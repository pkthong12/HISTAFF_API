using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class TimeLateEarlyDTO : Pagings
    {
        public long Id { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public string PositionName { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class TimeLateEarlyInputDTO
    {
        public long Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public long? EmployeeId { get; set; }
        public string Note { get; set; }


    }

    public class TimeLateEarlyPortalDTO
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public string Note { get; set; }
    }
}
