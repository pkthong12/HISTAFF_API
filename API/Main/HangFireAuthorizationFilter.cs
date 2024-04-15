using CoreDAL.Utilities;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.IdentityModel.Tokens.Jwt;

namespace API.Main
{
    public class HangFireAuthorizationFilter: IDashboardAuthorizationFilter
    {

        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
            /*
             * The code scope bellow work, but cross-domain cookies with samesite other than "none" are invisible
             */
            /*
            var httpContext = context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["HangFireCookie"];
            var handler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                var token = handler.ReadJwtToken(jwtToken);
                var jti = token.Claims.First(claim => claim.Type == "typ").Value;
                if (jti != null && jti != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            */
        }
    }
}
