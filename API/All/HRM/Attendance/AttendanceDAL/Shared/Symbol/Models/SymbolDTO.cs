using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace AttendanceDAL.ViewModels
{
    public class SymbolDTO 
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }      
        public bool? IsActive { get; set; }
        public string? IsActiveStr { get; set; }
        public bool? IsOff { get; set; }
        public bool? IsHaveSal { get; set; }
        public bool? IsHolidayCal { get; set; }
        public bool? IsInsArising { get; set; }
        public bool? IsPortal { get; set; }
        public bool? IsRegister { get; set; }
        public long? WorkingHour { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }

        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
