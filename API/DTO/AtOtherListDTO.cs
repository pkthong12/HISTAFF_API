using API.Main;

namespace API.DTO
{
    public class AtOtherListDTO: BaseDTO
    {
        public DateTime? EffectDate { get; set; }
        public string? EffectiveDateString { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsEntireYear { get; set; }
        public int? MaxWorkingMonth { get; set; }
        public int? MaxWorkingYear { get; set; }
        public double? OvertimeDayWeekday { get; set; }
        public double? OvertimeDayHoliday { get; set; }
        public double? OvertimeDayOff { get; set; }
        public double? OvertimeNightWeekday { get; set; }
        public double? OvertimeNightHoliday { get; set; }
        public double? OvertimeNightOff { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? StatusName { get; set; }
        public decimal? PersonalDeductionAmount { get; set; }
        public decimal? SelfDeductionAmount { get; set; }
        public decimal? BaseSalary { get; set; }
        public decimal? WorkdayUnitPrice { get; set; }
    }
}
