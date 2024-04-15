using CORE.Extension;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Main
{
    public static class HttpRequestExtensions
    {
        public static string Sid(this HttpRequest request, AppSettings _appSettings)
        {
            var token = request.Token();

            if (token == null)
                return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var sid = jwtToken.Claims.First(x => x.Type == "sid").Value;

                // return user id from JWT token if validation successful
                return sid;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }

        }

        public static string Typ(this HttpRequest request, AppSettings _appSettings)
        {
            var token = request.Token();

            if (token == null)
                return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var typ = jwtToken.Claims.First(x => x.Type == "typ").Value;

                // return username from JWT token if validation successful
                return typ;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }

        }
    }
}
