using Domain.Models;

namespace Application.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest mailRequest);

        Task ForgetPasswordSendMail(string toEmail, string username, string resetToken);
    }
}