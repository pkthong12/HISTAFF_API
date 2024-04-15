using API.All.DbContexts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using RegisterServicesWithReflection.Services.Base;
using API.Entities;

namespace API.All.SYSTEM.Common.Middleware
{
    [ScopedRegistration]
    public class JwtUtils: IJwtUtils
    {
        private readonly CoreDbContext _coreDbContext;
        private readonly AppSettings _appSettings;

        public JwtUtils(CoreDbContext coreDbContext, IOptions<AppSettings> appSettings)
        {
            _coreDbContext = coreDbContext;
            _appSettings = appSettings.Value;
        }

        public AccessTokenModel GenerateAccessToken(SYS_USER user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
            var expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtToken.WebMinutesOfLife);
            var utcMilliseconds = new DateTimeOffset(expires).ToUnixTimeMilliseconds();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("sid", user.ID),
                    new Claim("typ", user.USERNAME),
                    new Claim("iat", user.EMPLOYEE_ID.ToString()),
                    new Claim("IsAdmin", user.IS_ADMIN.ToString()),
                    new Claim("exp", expires.ToString()),
                    new Claim("iss", _appSettings.JwtToken.Issuer),
                    new Claim("aud", _appSettings.JwtToken.Audience)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return new AccessTokenModel()
            {
                AccessToken = tokenString,
                Sid = user.ID,
                Typ = user.USERNAME,
                Iat = user.EMPLOYEE_ID.ToString(),
                IsAdmin = user.IS_ADMIN.ToString(),
                Expires = expires
            };
        }

        public SYS_USER? GetUserFromAccessToken(string token)
        {
            if (token == null)
                return null;

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
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sid").Value;
                var user =  _coreDbContext.SysUsers.Find(userId);

                return user;
            }
            catch
            {
                return null;
            }
        }

        public SYS_REFRESH_TOKEN GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = new SYS_REFRESH_TOKEN
            {
                TOKEN = Convert.ToBase64String(randomBytes),
                EXPIRES = DateTime.UtcNow.AddDays(_appSettings.JwtToken.RefreshTokenDaysOfLife),
                CREATED = DateTime.UtcNow,
                CREATED_BY_IP = ipAddress
            };

            return refreshToken;
        }
    }
}
