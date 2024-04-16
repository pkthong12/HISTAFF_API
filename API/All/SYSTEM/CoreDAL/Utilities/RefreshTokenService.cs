using API;
using API.All.DbContexts;
using API.All.SYSTEM.Common;
using API.Entities;
using Microsoft.Extensions.Options;
using RegisterServicesWithReflection.Services.Base;
using System.Security.Cryptography;

namespace CoreDAL.Utilities
{

    public interface IRefreshTokenService
    {
        Task<SYS_REFRESH_TOKEN> UpdateRefreshTokens(string userID, string ipAddress, SYS_REFRESH_TOKEN refreshToken = null);
        Task<SYS_REFRESH_TOKEN> GenerateRefreshToken(string ipAddress, string userID);
        Task<CheckRefreshTokenResponse> CheckRefreshToken(string tokenString, string ipAddress);
        Task<SYS_USER?> GetUser(string ID);
        Task<SYS_USER?> GetUserByRefreshToken(string refreshTokenString);
    }

    [ScopedRegistration]
    public class RefreshTokenService : IRefreshTokenService
    {
        private RefreshTokenContext _refreshTokenContext;
        private readonly AppSettings _appSettings;

        public RefreshTokenService(RefreshTokenContext refreshTokenContext, IOptions<AppSettings> options)
        {
            _refreshTokenContext = refreshTokenContext;
            _appSettings = options.Value;
        }


        public async Task<SYS_REFRESH_TOKEN> UpdateRefreshTokens(string userID, string ipAddress, SYS_REFRESH_TOKEN refreshToken = null)
        {

            if (refreshToken == null) refreshToken = await GenerateRefreshToken(ipAddress, userID);
            await _refreshTokenContext.SysRefreshTokens.AddAsync(refreshToken);


            var user = await _refreshTokenContext.SysUsers.FirstAsync(x => x.ID == userID);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(userID);

            // save changes to db
            _refreshTokenContext.SysUsers.Update(user);
            await _refreshTokenContext.SaveChangesAsync();
            return refreshToken;

        }

        public async Task<SYS_REFRESH_TOKEN> GenerateRefreshToken(string ipAddress, string userID)
        {
            // generate token that is valid for 7 days
            var rngCryptoServiceProvider = await Task.Run(() => new RNGCryptoServiceProvider());
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = new SYS_REFRESH_TOKEN
            {
                USER = userID,
                TOKEN = Convert.ToBase64String(randomBytes),
                EXPIRES = DateTime.UtcNow.AddDays(7),
                CREATED = DateTime.UtcNow,
                CREATED_BY_IP = ipAddress
            };

            return refreshToken;
        }

        public async Task<CheckRefreshTokenResponse> CheckRefreshToken(string tokenString, string ipAddress)
        {
            var refreshToken = await _refreshTokenContext.SysRefreshTokens.AsNoTracking().SingleAsync(x => x.TOKEN == tokenString);
            var user = await GetUser(refreshToken.USER);

            if (user == null)
            {
                return new CheckRefreshTokenResponse() { Success = false, Message = CommonMessageCodes.NO_USER_MATCHS_PROVIDED_REFRESHTOKEN, NewRefreshToken = null };
            }

            var list = await (from p in _refreshTokenContext.SysRefreshTokens where p.USER == refreshToken.USER select p).ToListAsync();

            if (refreshToken.IS_REVOKED)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, list, ipAddress, $"Attempted reuse of revoked ancestor token: {tokenString}");
                _refreshTokenContext.UpdateRange(list);
                _refreshTokenContext.SaveChanges();
                return new CheckRefreshTokenResponse() { Success = false, Message = CommonMessageCodes.REVOKED_REFRESHTOKEN };
            }

            if (!refreshToken.IS_ACTIVE)
            {
                return new CheckRefreshTokenResponse() { Success = false, Message = CommonMessageCodes.INACTIVATED_REFRESHTOKEN };
            }

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = await rotateRefreshToken(refreshToken, ipAddress);
            _refreshTokenContext.Update(newRefreshToken);
            _refreshTokenContext.SaveChanges();


            return new CheckRefreshTokenResponse() { Success = true, Message = "", User = user, NewRefreshToken = newRefreshToken };
        }

        public async Task<SYS_USER?> GetUser(string ID)
        {
            var user = await (from p in _refreshTokenContext.SysUsers.Where(x => x.ID == ID) select p).FirstOrDefaultAsync();
            return user;
        }

        public async Task<SYS_USER?> GetUserByRefreshToken(string refreshTokenString)
        {
            var userID = await (from p in _refreshTokenContext.SysRefreshTokens.AsNoTracking().Where(x => x.TOKEN == refreshTokenString) select p.USER).FirstOrDefaultAsync();
            if (userID == null) return null;
            var user = await (from p in _refreshTokenContext.SysUsers.AsNoTracking().Where(x => x.ID == userID) select p).FirstOrDefaultAsync();
            return user;
        }

        private async Task<SYS_REFRESH_TOKEN> rotateRefreshToken(SYS_REFRESH_TOKEN refreshToken, string ipAddress)
        {
            var newRefreshToken = await GenerateRefreshToken(ipAddress, refreshToken.USER);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.TOKEN);
            return newRefreshToken;
        }


        private void RemoveOldRefreshTokens(string userID)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            try
            {
                var lst =  (from p in _refreshTokenContext.SysRefreshTokens.Where(x => x.USER == userID & DateTime.UtcNow >= x.EXPIRES & x.REVOKED==null & x.CREATED.AddDays(_appSettings.JwtToken.RefreshTokenTTL) <= DateTime.UtcNow) select p);

                _refreshTokenContext.SysRefreshTokens.RemoveRange(lst);
            }
            catch (Exception )
            {
            }


        }

        private void RevokeDescendantRefreshTokens(SYS_REFRESH_TOKEN refreshToken, List<SYS_REFRESH_TOKEN> list, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.REPLACED_BY_TOKEN))
            {
                var childToken = list.SingleOrDefault(x => x.TOKEN == refreshToken.REPLACED_BY_TOKEN);
                if (childToken != null)
                {
                    if (childToken.IS_ACTIVE)
                        RevokeRefreshToken(childToken, ipAddress, reason);
                    else
                        RevokeDescendantRefreshTokens(childToken, list, ipAddress, reason);
                }
            }
        }

        private static void RevokeRefreshToken(SYS_REFRESH_TOKEN token, string ipAddress, string? reason = null, string? replacedByToken = null)
        {
            token.REVOKED = DateTime.UtcNow;
            token.REVOKED_BY_IP = ipAddress;
            token.REASON_REVOKED = reason;
            token.REPLACED_BY_TOKEN = replacedByToken;
        }

    }
}
