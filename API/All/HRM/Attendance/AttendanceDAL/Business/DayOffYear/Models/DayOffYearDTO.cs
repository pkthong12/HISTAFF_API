using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class DayOffYearDTO : Pagings
    {
        public long Id { get; set; }
        public long? TenantID { get; set; }
        public long EmployeeId { get; set; }
        public int? YearId { get; set; }
        public int? DayOff { get; set; }
        public int? Month1 { get; set; }
        public int? Month2 { get; set; }
        public int? Month3 { get; set; }
        public int? Month4 { get; set; }
        public int? Month5 { get; set; }
        public int? Month6 { get; set; }
        public int? Month7 { get; set; }
        public int? Month8 { get; set; }
        public int? Month9 { get; set; }
        public int? Month10 { get; set; }
        public int? Month11 { get; set; }
        public int? Month12 { get; set; }
    }
    public class DayOffYearConfigDTO
    {
        public long? Id { get; set; }
        public bool? IsIntern { get; set; } // cho cong phep nam sau khi thu viec
        public bool? IsAccumulation { get; set; } // cho phep tich luy
        public int? MonthId { get; set; } // thoi han tich luy nam sau
    }

    public class EntertimentView
    {
        public decimal CurHave { get; set; }
        public decimal CurUsed { get; set; }
    }
}
