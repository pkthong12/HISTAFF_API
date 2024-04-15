using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.PORTAL
{
    [Table("PORTAL_REQUEST_CHANGE")]
    public class PORTAL_REQUEST_CHANGE:BASE_ENTITY
    {
        public long? EMPLOYEE_ID { get; set; }
        public long? ID_CHANGE { get; set; }
        public string? SYS_OTHER_CODE { get; set; }
        public string? CONTENT_CHANGE { get; set; }
        public string? REASON_CHANGE { get; set; }
        public long? IS_APPROVE { get; set; }
        public string? REASON_DISCARD { get; set; }
        public decimal? SAL_INSU { get; set; }
        public string? FILE_NAME { get; set; }
    }
}
