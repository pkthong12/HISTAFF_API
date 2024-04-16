namespace API.All.Services
{
    public interface IEmailService
    {
        Task SendEmail(string backGroundJobType);
        Task SendEmailAfterResetPassword(string mailTo, string mailFrom, string password);
    }
}
