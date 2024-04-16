using API.Main;

namespace API.DTO
{
    public class InsClaimInsuranceDTO : BaseDTO
    {
        public long? InsHealthId { get; set; }
        public long? SunCareId { get; set; }
        public DateTime? ExamineDate { get; set; }
        public string? DiseaseName { get; set; }
        public decimal? AmountOfClaims { get; set; }
        public decimal? AmountOfCompensation { get; set; }
        public DateTime? CompensationDate { get; set; }
        public string? Note { get; set; }
    }
}
