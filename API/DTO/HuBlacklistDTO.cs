using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class HuBlacklistDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public DateTime? DateJoin { get; set; }
        public DateTime? DateSendForm { get; set; }
        public DateTime? EndDateWork { get; set; }
        public DateTime? LastWorkingDay { get; set; }
        public long? StatusId { get; set; }
        public string? Note { get; set; }
        public string? ReasonForBlacklist { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeNo { get; set;}
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public string? StatusName { get; set; }
    }
}