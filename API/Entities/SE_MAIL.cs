using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SE_MAIL")]
    public class SE_MAIL : BASE_ENTITY
    {
        public string? SUBJECT { get; set; }
        public string? MAIL_FROM { get; set; }
        public string? MAIL_TO { get; set; }
        public string? MAIL_CC { get; set; }
        public string? MAIL_BCC { get; set; }
        public string? MAIL_CONTENT { get; set; }
        public string? VIEW_NAME { get; set; }
        public string? ACTFLG { get; set; }
        public string? ATTACHMENT { get; set; }
        public string? MAIL_LOG { get; set; }
        public DateTime? SEND_DATE { get; set; }
        public int? RUN_ROW { get; set; }
        public int? ORDER_BY { get; set; }
        public int? WAITING { get; set; }
    }
}
