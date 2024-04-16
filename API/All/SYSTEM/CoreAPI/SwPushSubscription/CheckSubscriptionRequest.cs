namespace API.All.SYSTEM.CoreAPI.SwPushSubscription
{
    public class CheckSubscriptionRequest
    {
        public required string UserId { get; set; }
        public required string Endpoint { get; set; }
    }
}
