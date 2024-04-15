namespace API.All.SYSTEM.CoreAPI.Authentication
{
    public class ClientLoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? AppType { get; set; }
    }
}
