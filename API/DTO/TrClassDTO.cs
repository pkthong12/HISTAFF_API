using API.Main;

namespace API.DTO
{
    public class TrClassDTO : BaseDTO
    {
        public long? TrProgramId { get; set; }
        public string? TrProgramName { get; set; }
        public string? Teacher { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public long? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public DateTime? TimeFrom { get; set; }
        public string? TimeFromStr { get; set; }
        public DateTime? TimeTo { get; set; }
        public string? TimeToStr { get; set; }
        public int? TotalDay { get; set; }
        public long? TotalTime { get; set; }
        public string? TotalTimeStr { get; set; }
        public string? Note { get; set; }
        public string? EmailContent { get; set; }
        public double? Ratio { get; set; }
    }
}
