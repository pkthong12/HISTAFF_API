using API.Socket;
using CORE.SignalHub;
using Microsoft.AspNetCore.SignalR;
using RegisterServicesWithReflection.Services.Base;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace API.All.Services
{
    [TransientRegistration]
    public class EmailService: IEmailService
    {
        public async Task SendEmail(string backGroundJobType)
        {
            List<string> emails = new() { "nvt5472@gmail.com", "datnguyenmst@gmail.com", "thang58kt4@gmail.com", "tanbk54@gmail.com" };

            MailMessage msg = new();
            emails.ForEach(email => msg.To.Add(new MailAddress(email)));
            
            msg.From = new MailAddress("uh1016341@miukafoto.com", "no_reply@miukafoto.com");
            msg.Subject = "MiukaFoto";
            msg.Body = $"Chào bạn" + "<br><br>" +
                backGroundJobType + "<br><br>" +
                "Đây là email gửi tự động với mục đích test hệ thống<br>";
            msg.IsBodyHtml = true;
            msg.BodyEncoding = Encoding.UTF8;

            using SmtpClient smtp = new();
            var credential = new NetworkCredential
            {
                UserName = "uh1016341",
                Password = "2WF1U4jwxd"
            };
            smtp.Credentials = credential;
            smtp.Host = "webmail.uh.ua";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(msg);

        }

        public async Task SendEmailAfterResetPassword(string mailTo, string mailFrom, string password)
        {
            MailMessage msg = new();
            msg.From = new MailAddress("uh1016341@miukafoto.com", "vinasteel@gmail.com");
            msg.To.Add(new MailAddress(mailTo));
            msg.Subject = "Email từ thao tác đặt lại mật khẩu mặc định";
            msg.Body = $"Mật khẩu mới của bạn là:" + "<br><br>" +
                "<strong>" + password + "</strong>" + "<br><br>" +
                "Bạn cần đổi mật khẩu trong lần đăng nhập đầu tiên";
            msg.IsBodyHtml = true;
            msg.BodyEncoding = Encoding.UTF8;

            using SmtpClient smtp = new();
            var credential = new NetworkCredential
            {
                UserName = "uh1016341",
                Password = "2WF1U4jwxd"
            };
            smtp.Credentials = credential;
            smtp.Host = "webmail.uh.ua";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(msg);

        }
    }
}
