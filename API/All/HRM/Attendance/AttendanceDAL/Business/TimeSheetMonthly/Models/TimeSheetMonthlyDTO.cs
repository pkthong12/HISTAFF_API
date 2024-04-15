using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace AttendanceDAL.ViewModels
{
    public class TimeSheetMonthlyDTO : Pagings
    {

        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
        public double? WorkingE { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public long? PeriodId { get; set; }
        public long? DecisionId { get; set; }
        public long? PaObjectSalaryId { get; set; }
        public long? StaffRankId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalWorkingXj { get; set; }
        public double? TotalTsV { get; set; }
        public double? TotalWorking { get; set; }
        public double? WorkingAdd { get; set; }
        public DateTime? DecisionStart { get; set; }
        public DateTime? DecisionEnd { get; set; }
        public double? WorkingTv { get; set; }
        public double? WorkingA { get; set; }
        public double? WorkingNh { get; set; }
        public double? WorkingR { get; set; }
        public double? WorkingS { get; set; }
        public double? WorkingNb { get; set; }
        public double? WorkingJ { get; set; }
        public double? WorkingTs { get; set; }
        public double? WorkingD { get; set; }
        public double? WorkingV { get; set; }
        public double? WorkingKt { get; set; }
        public double? WorkingL { get; set; }
        public double? WorkingDs { get; set; }
        public double? WorkingH { get; set; }
        public double? TotalNb { get; set; }
        public double? OtTotalConvert { get; set; }
        public double? TotalOtWeekday { get; set; }
        public double? TotalOtSunday { get; set; }
        public double? TotalOtSundaynigth { get; set; }
        public double? TotalOtHoliday { get; set; }
        public double? TotalOtHolidayNigth { get; set; }
        public double? TotalOtStore { get; set; }
        public double? WorkingX { get; set; }
        public double? WorkingF { get; set; }
        public double? WorkingQ { get; set; }
        public double? WorkingNv { get; set; }
        public double? WorkingP { get; set; }
        public double? WorkingO { get; set; }
        public double? WorkingDa { get; set; }
        public double? WorkingVs { get; set; }
        public double? WorkingRo { get; set; }
        public double? WorkingCt { get; set; }
        public double? WorkingKo { get; set; }
        public double? WorkingNh1 { get; set; }
        public double? OtTotalConvertPay { get; set; }
        public double? OtTotalConvertNb { get; set; }
        public double? TotalHl { get; set; }
        public double? TotalKl { get; set; }
        public double? TotalBhxh { get; set; }
        public double? TotalOffKl { get; set; }
        public double? TotalLateComebackoutDk { get; set; }
        public double? TotalLateComebackoutKdk { get; set; }
        public double? ChuyenCan { get; set; }
        public double? Ot150Nqd { get; set; }
        public double? Ot210Nqd { get; set; }
        public double? Ot200Nqd { get; set; }
        public double? Ot270Nqd { get; set; }
        public double? Ot300Nqd { get; set; }
        public double? Ot390Nqd { get; set; }
        public double? TotalPayOtNqd { get; set; }
        public double? TotalLc { get; set; }
        public double? TotalWorkingHl { get; set; }
        public double? TotalOtWeeknight { get; set; }
        public double? TotalShiftS3 { get; set; }
        public double? Ot150 { get; set; }
        public double? Ot210 { get; set; }
        public double? Ot200 { get; set; }
        public double? Ot270 { get; set; }
        public double? Ot300 { get; set; }
        public double? Ot390 { get; set; }
        public double? WorkingStandard { get; set; }
        public double? PMaxPayot { get; set; }
        public double? WorkingStandardGd { get; set; }
        public double? IsSancong { get; set; }
        public double? WorkingLcb { get; set; }
        public double? WorkingCtn { get; set; }
        public double? TotalLate { get; set; }
        public double? TotalComebackout { get; set; }
        public double? TotalDmPhat { get; set; }
        public double? TotalVsPhat { get; set; }
        public double? WorkingLOff { get; set; }
        public double? IsTrailwork { get; set; }
        public string ObjEmpName { get; set; }
        public double? TotalWorkingCt { get; set; } // gom nghi ct + ctn
        public long? Year { get; set; }
        public int? JobOrderNum { get; set; }
        public int? OrgOrderNum { get; set; }
        public bool? IsLock { get; set; }
    }
    public class TimeSheetInputDTO
    {
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public long? TitleId { get; set; }
        public double? WorkingE { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public long? PeriodId { get; set; }
        public long? DecisionId { get; set; }
        public long? PaObjectSalaryId { get; set; }
        public long? StaffRankId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalWorkingXj { get; set; }
        public double? TotalTsV { get; set; }
        public double? TotalWorking { get; set; }
        public double? WorkingAdd { get; set; }
        public DateTime? DecisionStart { get; set; }
        public DateTime? DecisionEnd { get; set; }
        public double? WorkingTv { get; set; }
        public double? WorkingA { get; set; }
        public double? WorkingNh { get; set; }
        public double? WorkingR { get; set; }
        public double? WorkingS { get; set; }
        public double? WorkingNb { get; set; }
        public double? WorkingJ { get; set; }
        public double? WorkingTs { get; set; }
        public double? WorkingD { get; set; }
        public double? WorkingV { get; set; }
        public double? WorkingKt { get; set; }
        public double? WorkingL { get; set; }
        public double? WorkingDs { get; set; }
        public double? WorkingH { get; set; }
        public double? TotalNb { get; set; }
        public double? OtTotalConvert { get; set; }
        public double? TotalOtWeekday { get; set; }
        public double? TotalOtSunday { get; set; }
        public double? TotalOtSundaynigth { get; set; }
        public double? TotalOtHoliday { get; set; }
        public double? TotalOtHolidayNigth { get; set; }
        public double? TotalOtStore { get; set; }
        public double? WorkingX { get; set; }
        public double? WorkingF { get; set; }
        public double? WorkingQ { get; set; }
        public double? WorkingNv { get; set; }
        public double? WorkingP { get; set; }
        public double? WorkingO { get; set; }
        public double? WorkingDa { get; set; }
        public double? WorkingVs { get; set; }
        public double? WorkingRo { get; set; }
        public double? WorkingCt { get; set; }
        public double? WorkingKo { get; set; }
        public double? WorkingNh1 { get; set; }
        public double? OtTotalConvertPay { get; set; }
        public double? OtTotalConvertNb { get; set; }
        public double? TotalHl { get; set; }
        public double? TotalKl { get; set; }
        public double? TotalBhxh { get; set; }
        public double? TotalOffKl { get; set; }
        public double? TotalLateComebackoutDk { get; set; }
        public double? TotalLateComebackoutKdk { get; set; }
        public double? ChuyenCan { get; set; }
        public double? Ot150Nqd { get; set; }
        public double? Ot210Nqd { get; set; }
        public double? Ot200Nqd { get; set; }
        public double? Ot270Nqd { get; set; }
        public double? Ot300Nqd { get; set; }
        public double? Ot390Nqd { get; set; }
        public double? TotalPayOtNqd { get; set; }
        public double? TotalLc { get; set; }
        public double? TotalWorkingHl { get; set; }
        public double? TotalOtWeeknight { get; set; }
        public double? TotalShiftS3 { get; set; }
        public double? Ot150 { get; set; }
        public double? Ot210 { get; set; }
        public double? Ot200 { get; set; }
        public double? Ot270 { get; set; }
        public double? Ot300 { get; set; }
        public double? Ot390 { get; set; }
        public double? WorkingStandard { get; set; }
        public double? PMaxPayot { get; set; }
        public double? WorkingStandardGd { get; set; }
        public double? IsSancong { get; set; }
        public double? WorkingLcb { get; set; }
        public double? WorkingCtn { get; set; }
        public double? TotalLate { get; set; }
        public double? TotalComebackout { get; set; }
        public double? TotalDmPhat { get; set; }
        public double? TotalVsPhat { get; set; }
        public double? WorkingLOff { get; set; }
        public double? IsTrailwork { get; set; }
        public List<long?>? OrgIds { get; set; }
        public int? StatusColex { get; set; }
    }


    public class TimeSheetPortal
    {
        public double WorkingX { get; set; }
        public double WorkingLpay { get; set; }
        public double WorkingPay { get; set; }
        public double WorkingN { get; set; }
        public double WorkingStand { get; set; }
        public List<TimeSheetPortalDtl> Detail { get; set; }

    }

    public class TimeSheetPortalDtl
    {
        public DateTime WorkingDay { get; set; }
        public string TimeTypeName { get; set; }
        public string TimePoint1 { get; set; }
        public string TimePoint4 { get; set; }
        public int? LateInEarlyOut { get; set; }
        public int? LateIn { get; set; }
        public int? EarlyOut { get; set; }
        public double? OT { get; set; }
    }

    public class SwipeImportnput
    {
        public List<SwipeDataInput> Data { get; set; }
        public int OrgId { get; set; }
        public int PeriodId { get; set; }
    }
    public class SwipeDataInput
    {
        public string CODE { get; set; }
        public string TIME { get; set; }
    }
    public class MaChineInput

    {
        public long? Id { get; set; }
        public string TIME_EDIT { get; set; }
        public string TYPE_EDIT { get; set; }
        public string NOTE { get; set; }
    }

    public class EntitlementDTO : Pagings
    {
        public int OrgId { get; set; }
        public int Year { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
    }

    public class RptAT001
    {
        public int Stt { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string OName { get; set; }
        public string PName { get; set; }
        public string WorkingDay { get; set; }
        public string TCode { get; set; }
        public string HoursStart { get; set; }
        public string HoursStop { get; set; }
        public string TimePoint1 { get; set; }
        public string TimePoint4 { get; set; }
        public int? LateIn { get; set; }
        public int? EarlyOut { get; set; }
        public string OtStart { get; set; }
        public string OtEnd { get; set; }
        public int? OtTime { get; set; }
        public int? OtTimeNight { get; set; }
    }

    public class LockDataInput
    {
        public List<long>? OrgIds { get; set; }
        public long? PeriodId { get; set; }
    }
}
