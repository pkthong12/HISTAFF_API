using API.Main;

namespace API.DTO
{
    public class HuPositionOrgMapBuffer:BaseDTO
    {
        public string? UserId { get; set; }
        public long? OldPositionId { get; set; }
        public long? NewPositionId { get; set; }
        public long? OrgId { get; set; }
    }
}
