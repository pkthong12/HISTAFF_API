using CORE.Services.File;
using ProfileDAL.ViewModels;

namespace API.DTO
{
    public class AtDeclareSeniorityDTO
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public long? YearDeclare { get; set; }

        public long? MonthAdjust { get; set; }
        public string? MonthAdjustName { get; set; }

        public long? MonthAdjustNumber { get; set; }

        public string? ReasonAdjust { get; set; }

        public long? MonthDayOff { get; set; }

        public string? MonthDayOffName { get; set; }

        public double? NumberDayOff { get; set; }

        public string? ReasonAdjustDayOff { get; set; }

        public double? Total { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public long? OrgId { get; set; }

        public string? EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
