using API.Entities;

namespace API.All.SYSTEM.Common.Middleware
{
    public interface IJwtUtils
    {
        public AccessTokenModel GenerateAccessToken(SYS_USER user);
        public SYS_USER? GetUserFromAccessToken(string token);
        public SYS_REFRESH_TOKEN GenerateRefreshToken(string ipAddress);
    }
}
