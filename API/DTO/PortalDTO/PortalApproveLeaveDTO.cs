using API.Main;

namespace API.DTO.PortalDTO
{
    public class PortalApproveLeaveDTO : BaseDTO
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? WorkingDay { get; set; }
        public long? TimeTypeId { get; set; }
        public string? TimeTypeName { get; set; }
        public long? ApproveId { get; set; }
        public string? Note { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public DateTime? SendDate { get; set; }
        public string? TypeCode { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool? IsChangeToFund { get; set; }
        public bool? IsTimeKeeping { get; set; }
        public bool? IsWorkingOvernight { get; set; }
        public long? AppStatus { get; set; }
        public string? AppNote { get; set; }
        public int? AppLevel { get; set; }
        public int? TotalDay { get; set; }
        public decimal? TotalOt { get; set; }
        public string? TotalTimeOt { get; set; }
        public string? StatusApprove { get; set; }
        public List<int>? Statuses { get; set; }
        public long? TypeId { get; set; }
        public double? Late { get; set; }
        public double? Comebackout { get; set; }
        public List<PortalRegisterOffDetailDTO>? ListDetail { get; set; }

        // Search
        public DateTime? DateStartSearch { get; set; }
        public DateTime? DateEndSearch { get; set; }

        // 
        public long? RefId { get; set; }
    }
}
