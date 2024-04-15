using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace AttendanceDAL.ViewModels
{
    public class TimeSheetLockDTO : Pagings
    {
        public long Id { get; set; }
        //public long? TenantID { get; set; }
        public long PeriodId { get; set; }
        public long OrgId { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class TimeSheetLockInputDTO
    {
        [Required(ErrorMessage = "{0}_Required")]
        public long PERIOD_ID { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long ORG_ID { get; set; }
    }
}
