using CORE.DTO;
using Microsoft.AspNetCore.Mvc;
using static CoreAPI.AuthenticationController;

namespace API.All.SYSTEM.CoreAPI.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public Task<FormatedResponse> ClientsLogin([FromBody] ClientLoginRequest request, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> RefreshToken(RefreshTokenObject? refreshTokenObject)
        {
            throw new NotImplementedException();
        }
    }
}
