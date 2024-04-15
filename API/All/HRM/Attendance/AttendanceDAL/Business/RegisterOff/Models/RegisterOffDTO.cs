using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class RegisterOffDTO : Pagings
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public int? PositionId { get; set; }
        public long TypeId { get; set; } // Loại đăng ký
        public string PositionName { get; set; }
        public long? TimetypeId { get; set; }
        public string TimetypeCode { get; set; }
        public int? PeriodId { get; set; }
        public string PeriodName { get; set; }
        public long? StatusId { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public DateTime? WorkingDay { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Note { get; set; }
    }
    public class RegisterOffInputDTO
    {
        public long? Id { get; set; }
        public long? TimeTypeId { get; set; }
        public List<long> EmpIds { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Note { get; set; }
        public string WorkingDay { get; set; }// bỏ
    }

    public class PortalRegInputDTO
    {
        public long? TimeTypeId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Note { get; set; }
        public string WorkingDay { get; set; }// bỏ

    }
    public class PortalApproveDTO
    {
        public long Id { get; set; }
        public string Note { get; set; }
    }

    public class PortalEntiParam
    {
        public decimal? CurUsedMonth { get; set; }
        public decimal? Remain { get; set; }
    }

    public class PortalDMVSParam
    {
        public decimal? ViolationTotal { get; set; }
        public decimal? ViolationTime { get; set; }
    }

    public class DateSearchParam : Pagings
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }

    public class HistoryRegView
    {
        public long Id { get; set; }
        public string? IdReggroup { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? TypeId { get; set; }
        public long? TimeEarly { get; set; }
        public long? TimeLate { get; set; }
        public string? Name { get; set; }
        public string? TimeStart { get; set; }
        public string? TimeEnd { get; set; }
        public decimal? TotalOt { get; set; }
        public int? TotalDay { get; set; }
        public string? Note { get; set; }
        public long? StatusId { get; set;}
    }
}
