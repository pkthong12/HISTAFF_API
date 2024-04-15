using API.Main;

namespace API.DTO
{
    public class PaPeriodTaxDTO : BaseDTO
    {
        public string? TaxMonth { get; set; }
        public int? Year { get; set; }
        public long? MonthlyTaxCalculation { get; set; }
        public long? PeriodId { get; set; }
        public DateTime? TaxDate { get; set; }
        public DateTime? CalculateTaxFromDate { get; set; }
        public DateTime? CalculateTaxToDate { get; set;}
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }
}
