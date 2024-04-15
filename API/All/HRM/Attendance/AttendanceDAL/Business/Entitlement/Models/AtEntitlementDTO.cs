using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class AtEntitlementDTO : Pagings
    {
         public long? Id { get; set; }
        public long? Year { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public double? WorkingTimeHave { get; set; }
        public double? PrevHave { get; set; }
        public double? PrevUsed { get; set; }
        public double? CurHave { get; set; }
        public double? CurUsed { get; set; }
        public double? CurHave1 { get; set; }
        public double? CurHave2 { get; set; }
        public double? CurHave3 { get; set; }
        public double? CurHave4 { get; set; }
        public double? CurHave5 { get; set; }
        public double? CurHave6 { get; set; }
        public double? CurHave7 { get; set; }
        public double? CurHave8 { get; set; }
        public double? CurHave9 { get; set; }
        public double? CurHave10 { get; set; }
        public double? CurHave11 { get; set; }
        public double? CurHave12 { get; set; }
        public double? CurUsed1 { get; set; }
        public double? CurUsed2 { get; set; }
        public double? CurUsed3 { get; set; }
        public double? CurUsed4 { get; set; }
        public double? CurUsed5 { get; set; }
        public double? CurUsed6 { get; set; }
        public double? CurUsed7 { get; set; }
        public double? CurUsed8 { get; set; }
        public double? CurUsed9 { get; set; }
        public double? CurUsed10 { get; set; }
        public double? CurUsed11 { get; set; }
        public double? CurUsed12 { get; set; }
         public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
        public DateTime? JoinDate { get; set; }
        public double? QpMonthSum { get; set; }
        public double? QpYearSum { get; set; }
        public double? SeniorityMonth { get; set; }
        public double? Seniority { get; set; }
         public double? SeniorityHave { get; set; }
        public DateTime? Expiredate { get; set; }
        public double? PrevtotalHave { get; set; }
        public double? QpYear { get; set; }
         public double? TotalHave { get; set; }
        public double? QpYearXUsed { get; set; }
        public double? QpYearXHave { get; set; }
        public double? QpStandard { get; set; }
        public int? JobOrderNum { get; set; }

    }
    public class AtEntitlementInputDTO
    {
         public long? Id { get; set; }
        public long? Year { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public double? WorkingTimeHave { get; set; }
        public double? PrevHave { get; set; }
        public double? PrevUsed { get; set; }
        public double? CurHave { get; set; }
        public double? CurUsed { get; set; }
        public double? CurHave1 { get; set; }
        public double? CurHave2 { get; set; }
        public double? CurHave3 { get; set; }
        public double? CurHave4 { get; set; }
        public double? CurHave5 { get; set; }
        public double? CurHave6 { get; set; }
        public double? CurHave7 { get; set; }
        public double? CurHave8 { get; set; }
        public double? CurHave9 { get; set; }
        public double? CurHave10 { get; set; }
        public double? CurHave11 { get; set; }
        public double? CurHave12 { get; set; }
        public double? CurUsed1 { get; set; }
        public double? CurUsed2 { get; set; }
        public double? CurUsed3 { get; set; }
        public double? CurUsed4 { get; set; }
        public double? CurUsed5 { get; set; }
        public double? CurUsed6 { get; set; }
        public double? CurUsed7 { get; set; }
        public double? CurUsed8 { get; set; }
        public double? CurUsed9 { get; set; }
        public double? CurUsed10 { get; set; }
        public double? CurUsed11 { get; set; }
        public double? CurUsed12 { get; set; }
         public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
    }

    
}
