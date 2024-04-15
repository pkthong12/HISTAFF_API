using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace AttendanceDAL.ViewModels
{
    public class HolidayDTO 
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDayoff { get; set; }
        public DateTime? EndDayoff { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Status { get; set; }
    }
}
