using API.Main;

namespace API.DTO
{
    public class AtSalaryPeriodDTO:BaseDTO
    {
        public string? Name { get; set; }
        public int Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StandardWorking { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public int? Month { get; set; }
        public string? Actflg { get; set; }

        // REalated/Extra

        public List<long>? OrgIds { get; set; }
        public List<AT_ORG_PERIOD>? OrgPeriods { get; set; }
        public long? PeriodId { get; set; }
        public long? OrgId { get; set; }

    }
}
