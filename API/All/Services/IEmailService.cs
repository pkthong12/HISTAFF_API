using API.DTO;

namespace API.All.Services
{
    public interface IEmailService
    {
        Task SendEmail(string backGroundJobType);
        Task SendEmailAfterResetPassword(string mailTo, SeConfigDTO seConfigDto, string password);
    }
}
