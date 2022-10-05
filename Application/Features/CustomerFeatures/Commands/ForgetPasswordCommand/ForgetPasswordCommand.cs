using Application.Services;
using MediatR;
using Persistence.Context;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CustomerFeatures.Commands.ForgetPasswordCommand
{
    public class ForgetPasswordCommand : IRequest<ForgetPasswordViewModel>
    {
        [EmailAddress]
        public string Email { get; set; }

        public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ForgetPasswordViewModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMailService _mailService;

            public ForgetPasswordCommandHandler(ApplicationDbContext context, IMailService mailService)
            {
                _context = context;
                _mailService = mailService;
            }

            public async Task<ForgetPasswordViewModel> Handle(ForgetPasswordCommand command, CancellationToken token)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == command.Email);
                if (user == null)
                    return new ForgetPasswordViewModel
                    {
                        IsSuccess = false,
                        Message = "Invalid email"
                    };
                var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var arr = new char[40];
                var random = new Random();
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = characters[random.Next(characters.Length)];
                }
                var resetToken = new String(arr);

                user.ResetToken = resetToken;
                user.ResetTokenExpiryDate = DateTime.Now.AddMinutes(15);
                await _context.SaveChangesAsync();

                await _mailService.ForgetPasswordSendMail(user.Email, user.Username, resetToken);

                return new ForgetPasswordViewModel
                {
                    IsSuccess = true,
                    Message = "Check your mail to reset password"
                };
            }
        }
    }
}