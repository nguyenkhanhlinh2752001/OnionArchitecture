using Application.Helper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.CustomerFeatures.Commands.LoginCommand
{
    public class LoginCommand : IRequest<LoginCommandViewModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandViewModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IConfiguration _config;

            public LoginCommandHandler(ApplicationDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
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
                var validToken = CreateToken(currentUser.Username);
                return new LoginCommandViewModel
                {
                    IsSuccess = true,
                    Message = validToken
                };
            }

            public string CreateToken(string username)
            {
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.NameId, username)
                };
                var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256Signature),
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }
}