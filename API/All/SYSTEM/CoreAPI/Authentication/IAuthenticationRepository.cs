using CORE.DTO;
using Microsoft.AspNetCore.Mvc;
using static CoreAPI.AuthenticationController;

namespace API.All.SYSTEM.CoreAPI.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<FormatedResponse> ClientsLogin([FromBody] ClientLoginRequest request, string ipAddress);
        Task<FormatedResponse> RefreshToken(RefreshTokenObject? refreshTokenObject);
    }
}
