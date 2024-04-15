using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SYS_MAIL_TEMPLATE")]
    public class SYS_MAIL_TEMPLATE: BASE_ENTITY
    {
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public string? MAIL_CC { get; set; }
        public string? REMARK { get; set; }
        public string? GROUP_MAIL { get; set; }
        public string? CONTENT { get; set; }
        public int SEND_FIXED { get; set; }
        public string? SEND_TO { get; set; }
        public bool IS_MAIL_CC { get; set; }
        public long? FUNCTIONAL_GROUP_ID { get; set; }
        public string? TITLE { get; set; }
    }
}
