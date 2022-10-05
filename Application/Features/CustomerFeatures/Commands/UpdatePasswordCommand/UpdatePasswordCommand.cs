using Application.Helper;
using MediatR;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordCommand : IRequest<UpdatePasswordViewModel>
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, UpdatePasswordViewModel>
        {
            private readonly ApplicationDbContext _context;

            public UpdatePasswordCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<UpdatePasswordViewModel> Handle(UpdatePasswordCommand command, CancellationToken token)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == command.Username);
                var currentPasswordHash = EnCodeHelper.EnCodeSha1(command.CurrentPassword);
                for (int i = 0; i < currentPasswordHash.Length; i++)
                {
                    if (user.PasswordHash[i] != currentPasswordHash[i])
                        return new UpdatePasswordViewModel
                        {
                            IsSuccess = false,
                            Message = "Invalid current password"
                        };
                }
                user.PasswordHash = EnCodeHelper.EnCodeSha1(command.NewPassword);
                await _context.SaveChangesAsync();
                return new UpdatePasswordViewModel
                {
                    IsSuccess = true,
                    Message = "Change password successfully"
                };
            }
        }
    }
}