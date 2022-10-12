using Domain.Models;

namespace Persistence.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task<bool> ForgetPasswordSendMail(string toEmail, string username, string resetToken);
    }
}