using API.Main;

namespace API.DTO
{
    public class PaPhaseAdvanceDTO : BaseDTO
    {

        public long? PeriodId { get; set; }
        public string? PeriodName { get;set; }
        public long? OrgId { get; set; }
        public string? OrgName {  get; set; }
        public string? Actflg { get; set; }
        public string? Note { get; set; }
        public string? Symbol { get; set; }
        public int? PhaseNameId { get; set; }
        public double? MonthLbs { get; set; }
        public double? Seniority { get; set; }
        public string? MonthLbsName { get; set; }
        public long? SymbolId { get; set; }
        public string? SymbolName { get; set; }
        public bool? IsActive { get; set; }
        public string? NameVn { get; set; }
        public int? Year { get; set; }
        public string? YearPeriod { get; set; }
        public DateTime? PhaseDay { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
        //public List<long>? SymbolIds { get; set; }
        public List<long?> ListSymbolId { get; set; }

        public long? FromSalary { get; set; }
        public string? FromSalaryName { get; set; }
        public long? ToSalary { get; set; }
        public string? ToSalaryName { get; set; }


    }
}
