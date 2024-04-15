namespace API.All.SYSTEM.CoreAPI.Authentication
{
    public class SsoExchangeCodeRequest
    {
        public required string ClientId { get; set; }
        public required string AuthorizationCode { get; set; }
        public required string CodeVerifier { get; set; }
    }
}
