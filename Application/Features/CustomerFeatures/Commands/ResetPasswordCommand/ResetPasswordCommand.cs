using Application.Helper;
using MediatR;
using Persistence.Context;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CustomerFeatures.Commands.ForgetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<ResetPasswordViewModel>
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordViewModel>
        {
            private readonly ApplicationDbContext _context;

            public ResetPasswordCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ResetPasswordViewModel> Handle(ResetPasswordCommand command, CancellationToken token)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == command.Email);
                var resetToken = user.ResetToken;
                if (user.ResetTokenExpiryDate < DateTime.Now)
                {
                    return new ResetPasswordViewModel
                    {
                        IsSuccess = false,
                        Message = "Expiried reset token"
                    };
                }
                for (int i = 0; i < resetToken.Length; i++)
                {
                    if (user.ResetToken[i] != resetToken[i])
                        return new ResetPasswordViewModel
                        {
                            IsSuccess = false,
                            Message = "Wrong reset token "
                        };
                }
                if (command.ConfirmPassword != command.NewPassword)
                    return new ResetPasswordViewModel
                    {
                        IsSuccess = false,
                        Message = "Confirm password does not match new password"
                    };
                user.PasswordHash = EnCodeHelper.EnCodeSha1(command.NewPassword);
                await _context.SaveChangesAsync();
                return new ResetPasswordViewModel
                {
                    IsSuccess = true,
                    Message = "Change password successfully"
                };
            }
        }
    }
}