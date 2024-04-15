using API.Entities;

namespace CoreDAL.Utilities
{
    public class CheckRefreshTokenResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public SYS_USER? User { get; set; }
        public SYS_REFRESH_TOKEN? NewRefreshToken { get; set; }
    }
}
