using API.Main;

namespace API.DTO
{
    public class SysMailTemplateDTO:BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? MailCc { get; set; }
        public string? Remark { get; set; }
        public string? GroupMail { get; set; }
        public string? Content { get; set; }
        public int? SendFixed { get; set; }
        public string? SendTo { get; set; }
        public bool? IsMailCc { get; set; }
        public long? FunctionalGroupId { get; set; }
        public string? FunctionalGroupName { get; set; }
    }
}
