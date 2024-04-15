using API.All.SYSTEM.Common.Middleware;
using System.IdentityModel.Tokens.Jwt;

namespace API.All.SYSTEM.Common.StaticClasses
{
    public static class JwtHelper
    {
        public static AccessTokenModel GetTokenInfo(string idtoken)
        {
            try
            {
                var token = new JwtSecurityToken(jwtEncodedString: idtoken);
                var jwtTokenModel = new AccessTokenModel
                {
                    Expires = DateTimeOffset.FromUnixTimeSeconds(long.Parse(token.Claims.First(c => c.Type == "exp").Value)).DateTime,
                    Iat = token.Claims.FirstOrDefault(c => c.Type == "iat").Value,
                    Sid = token.Claims.FirstOrDefault(c => c.Type == "sid").Value,
                    IsAdmin = token.Claims.FirstOrDefault(c => c.Type == "IsAdmin").Value,
                    Typ = token.Claims.FirstOrDefault(c => c.Type == "typ").Value
                };
                return jwtTokenModel;
            } catch {
                return null;
            }
        }
    }
}
