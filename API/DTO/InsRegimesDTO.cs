using API.Main;

namespace API.DTO
{
    public class InsRegimesDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? InsGroupId { get; set; }
        public long? CalDateType { get; set; }
        public string? InsGroupName { get; set; }
        public string? CalDateTypeString { get; set; }
        public int? TotalDay { get; set; }
        public int? BenefitsLevels { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public List<long>? ids { get; set; }
        public bool? ValueToBind { get; set; }
    }
}
