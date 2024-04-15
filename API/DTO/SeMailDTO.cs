using API.Main;

namespace API.DTO
{
    public class SeMailDTO : BaseDTO
    {
        public string? MailFrom{ get; set; }
        public string? MailTo{ get; set; }
        public string? MailCc{ get; set; }
        public string? MailBcc{ get; set; }
        public string? MailContent{ get; set; }
        public string? ViewName{ get; set; }
        public string? Actflg{ get; set; }
        public string? Attachment{ get; set; }
        public string? MailLog{ get; set; }
        public DateTime? SendDate{ get; set; }
        public int? RunRow{ get; set; }
        public int? OrderBy{ get; set; }
    }
}
