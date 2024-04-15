using API.Main;

namespace API.DTO
{
    public class AtTimeTimesheetDailyDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
        public DateTime? Workingday { get; set; }
        public DateTime? WorkingdayTo { get; set; }
        public string? ShiftCode { get; set; }
        public DateTime? ShiftStart { get; set; }
        public string? ShiftStartString { get; set; }
        public DateTime? ShiftEnd { get; set; }
        public string? ShiftEndString { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? LeaveCode { get; set; }
        public double? Late { get; set; }
        public double? Comebackout { get; set; }
        public double? WorkdayOt { get; set; }
        public double? WorkdayNight { get; set; }
        public string? TypeDay { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public DateTime? Valin1 { get; set; }
        public string? Valin1String { get; set; }
        public DateTime? Valin2 { get; set; }
        public DateTime? Valin3 { get; set; }
        public DateTime? Valin4 { get; set; }
        public string? Valin4String { get; set; }

        public long? DecisionId { get; set; }
        public long? PaObjectSalaryId { get; set; }
        public long? ShiftId { get; set; }
        public long? ManualId { get; set; }
        public string? ManualCode { get; set; }
        public long? LeaveId { get; set; }
        public double? Workinghour { get; set; }
        public double? WorkinghourShift { get; set; }
        public long? NumberSwipe { get; set; }
        public double? MinuteDmVipham { get; set; }
        public bool? IsGiaitrinh { get; set; }
        public double? ManualIdTk { get; set; }
        public double? LeaveIdTk { get; set; }
        public string? LeaveCodeTk { get; set; }
        public double? DimuonVesomThucte { get; set; }
        public double? OtWeekday { get; set; }
        public double? OtWeeknight { get; set; }
        public double? OtSunday { get; set; }
        public double? OtSundaynight { get; set; }
        public double? OtHoliday { get; set; }
        public double? OtHolidayNight { get; set; }
        public double? OtTotalConvert { get; set; }
        public double? OtWeekdayTc { get; set; }
        public double? OtWeeknightTc { get; set; }
        public double? OtSundayTc { get; set; }
        public double? OtSundaynightTc { get; set; }
        public double? OtHolidayTc { get; set; }
        public double? OtHolidayNightTc { get; set; }
        public double? OtWeekdaySc { get; set; }
        public double? OtWeeknightSc { get; set; }
        public double? OtSundaySc { get; set; }
        public double? OtSundaynightSc { get; set; }
        public double? OtHolidaySc { get; set; }
        public double? OtHolidayNightSc { get; set; }
        public bool? IsNbTc { get; set; }
        public bool? IsNbSc { get; set; }
        public double? OtTotalConvertPay { get; set; }
        public double? OtTotalConvertNb { get; set; }
        public bool? IsEdit { get; set; }
        public double? MinuteOut { get; set; }
        public string? HrComment { get; set; }
        public string? ModifiedDetail { get; set; }
        public bool? IsRegisterLate { get; set; }
        public bool? IsRegisterComebackout { get; set; }
        public double? TotalMinuteOut { get; set; }
        public bool? IsNoLateComebackout { get; set; }
        public bool? IsConfirm { get; set; }
        public double? MinuteDmDk { get; set; }
        public double? MinuteVsDk { get; set; }
        public long? ShiftIdOt { get; set; }
        public bool? IsEditInout { get; set; }
        public bool? IsOutRegister { get; set; }
        public bool? IsAnCa { get; set; }
        public bool? IsHoliday { get; set; }
        public bool? IsHolidayNext { get; set; }
        public bool? IsHolidayLt { get; set; }
        public bool? IsOffPre { get; set; }
        public bool? IsOffNext { get; set; }
        public double? GioVeMuon { get; set; }
        public double? OtWeekdayTt { get; set; }
        public double? OtWeeknightTt { get; set; }
        public double? OtSundayTt { get; set; }
        public double? OtSundaynightTt { get; set; }
        public double? OtHolidayTt { get; set; }
        public double? OtHolidayNightTt { get; set; }
        public DateTime? Valin1Tt { get; set; }
        public DateTime? Valin4Tt { get; set; }
        public long? ShiftManualId { get; set; }
        public DateTime? ValIn { get; set; }
        public DateTime? ValOut { get; set; }
        public bool? IsOffDn1 { get; set; }
        public bool? IsCaseOt { get; set; }
        public string? Reason { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public long[]? EmployeeIds { get; set; }
        public long? PeriodId { get; set; }

        public string? ReasonText { get; set; }
        public DateTime? ExplainDate { get; set; }
        public List<long?>? ListOrgIds { get; set; }
        public List<long?>? Ids { get; set; }

        public long? CodeColor { get; set; }

        public string? CodeColorStr { get; set; }

        public int? JobOrderNum { get; set; }

        // USING TO SEARCH
        public string? EmployeeCodeSearch { get; set; }

        public string? EmployeeNameSearch { get; set; }

        public string? DepartmentNameSearch { get; set; }

        public string? PositionNameSearch { get; set; }

        public string? SalPeriodObjSearch { get; set; }
    }

    public class AtTimesheetDailyImportDTO
    {

        public long? Id { get; set; }
        public long? ManualId { get; set; }
        public DateTime? Workingday { get; set; }
        public DateTime? WorkingdayTo { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
    }
}
