using API.Main;

namespace API.DTO
{
    public class SwPushSubscriptionDTO : BaseDTO
    {
        public string? UserId { get; set; }
        public string? Endpoint { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public string? Subscription { get; set; }

    }
}