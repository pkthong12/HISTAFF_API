using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace AttendanceDAL.ViewModels
{
    public class OverTimeDTO : Pagings
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public long TimetypeId { get; set; }
        public string TimetypeCode { get; set; }
        //public int? PeriodId { get; set; }
        //public string PeriodName { get; set; }
        public long? StatusId { get; set; }
        public DateTime? WorkingDay { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Note { get; set; }


    }
    public class OverTimeCreateDTO
    {
        public List<long> EmpIds { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime WorkingDay { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? TimeStart { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? TimeEnd { get; set; }
        public string Note { get; set; }

    }
    public class OverTimeInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long EmployeeId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime TimeStart { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime TimeEnd { get; set; }
        public DateTime WorkingDay { get; set; }
        public string Note { get; set; }

    }
    public class OverTimeConfigDTO
    {
        public long? Id { get; set; }
        public int? HourMin { get; set; }
        public int? HourMax { get; set; }
        public Decimal? FactorNt { get; set; }
        public Decimal? FactorNn { get; set; }
        public Decimal? FactorNl { get; set; }
        public Decimal? FactorDnt { get; set; }
        public Decimal? FactorDnn { get; set; }
        public Decimal? FactorDnl { get; set; }
    }

    public class OverTimePortalDTO
    {
        public DateTime WorkingDay { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Note { get; set; }
    }
    public class AcceptDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
        public int? Status { get; set; }
        public DateTime? Date { get; set; }
        public string Avatar { get; set; }
        public string OrgName { get; set; }
    }

    public class HistoryRegDTO
    {
        public long Id { get; set; }
        public int? TypeId { get; set; }
        public int? Status { get; set; }
        public DateTime? Date { get; set; } // build xong IOS bỏ
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
