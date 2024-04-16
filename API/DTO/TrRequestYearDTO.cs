using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class TrRequestYearDTO : BaseDTO
    {
        public int? Year { get; set; }
        public long? QuarterId { get; set; }
        public long? OrgId { get; set; }
        public DateTime? DateOfRequest { get; set; }
        public long? TrCourseId { get; set; }
        public string? Content { get; set; }
        public long? CompanyId { get; set; }
        public string? Participants { get; set; }
        public int? QuantityPeople { get; set; }
        public long? InitializationLocation { get; set; }
        public long? PriorityLevel { get; set; }
        public long? StatusId { get; set; }
        public decimal? Money { get; set; }
        public string? Note { get; set; }
        public string? OrgName { get; set; }
        public string? TrCourseName { get; set; }
        public string? CompanyName { get; set; }
        public string? QuarterName { get; set; }
        public string? InitializationLocationStr { get; set; }
        public string? PriorityLevelStr { get; set; }
        public string? StatusName { get; set; }
    }
}