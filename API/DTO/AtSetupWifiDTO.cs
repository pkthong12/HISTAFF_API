using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace API.DTO
{
    public class AtSetupWifiDTO : BaseDTO
    {
        public string? NameVn { get; set; }
        public string? NameWifi { get; set; }
        public string? Ip { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? OrgId { get; set; }
        public string? Status { get; set; }
        public string? OrgName { get; set; }
        public string? Note { get; set; }

    }
}
