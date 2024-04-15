using API.Main;

namespace API.DTO
{
    public class AtPeriodStandardDTO : BaseDTO
    {
        public int? Year { get; set; }

        public long? PeriodId { get; set; }

        public string? PeriodName { get; set; } 

        public long? ObjectId { get; set; }

        public string? ObjectName { get; set; }

        public int? PeriodStandard { get; set; }

        public int? PeriodStandardNight { get; set; }

        public bool? IsActive { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }
    }
}
