using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SE_PROCESS")]
    public class SE_PROCESS: BASE_ENTITY
    {
        public string CODE { get; set; } = null!;
        public string? NAME { get; set; }
        public long? PROCESS_TYPE_ID { get; set; }
        public string? APPROVED_CONTENT { get; set; }
        public string? APPROVED_SUCESS_CONTENT { get; set; }
        public string? NOT_APPROVED_CONTENT { get; set; }
        public bool? IS_NOTI_APPROVE { get; set; }
        public bool? IS_NOTI_APPROVE_SUCCESS { get; set; }
        public bool? IS_NOTI_NOT_APPROVE { get; set; }
        public string? PRO_DESCRIPTION { get; set; }
        public string? APPROVE { get; set; }
        public string? REFUSE { get; set; }
        public string? ADJUSTMENT_PARAM { get; set; }
    }
}
