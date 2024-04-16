using API.Main;

namespace API.DTO
{
    public class AtSetupGpsDTO : BaseDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? LatVd { get; set; }
        public string? LongKd { get; set; }
        public long? Radius { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? OrgId { get; set; }
        public string? Status { get; set; }
        public string? OrgName { get; set; }
    }
}
