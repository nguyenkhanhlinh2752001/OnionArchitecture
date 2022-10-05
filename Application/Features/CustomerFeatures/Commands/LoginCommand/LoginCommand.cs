using Application.Helper;
using Application.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Commands.LoginCommand
{
    public class LoginCommand : IRequest<LoginCommandViewModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandViewModel>
        {
            private readonly ApplicationDbContext _context;

            public LoginCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<LoginCommandViewModel> Handle(LoginCommand command, CancellationToken token)
            {
                command.Username = command.Username.ToLower();
                var currentUser = _context.Users.FirstOrDefault(u => u.Username == command.Username);
                if (currentUser == null)
                    return new LoginCommandViewModel
                    {
                        IsSuccess = false,
                        Message = "Invalid username"
                    };

                var passwordHash = EnCodeHelper.EnCodeSha1(command.Password);
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (currentUser.PasswordHash[i] != passwordHash[i])
                        return new LoginCommandViewModel
                        {
                            IsSuccess = false,
                            Message = "Invalid password"
                        };
                }
                var role = (from u in _context.Users
                            join ur in _context.UserRoles
                            on u.Id equals ur.UserId
                            join r in _context.Roles
                            on ur.RoleId equals r.Id
                            where u.Username == currentUser.Username
                            select new RoleVM
                            {
                                Id = r.Id,
                            }).FirstOrDefault();

                var validToken = TokenHelper.GenerateToken(currentUser.Username, role.Id);
                return new LoginCommandViewModel
                {
                    IsSuccess = true,
                    Message = validToken
                };
            }
        }
    }
}