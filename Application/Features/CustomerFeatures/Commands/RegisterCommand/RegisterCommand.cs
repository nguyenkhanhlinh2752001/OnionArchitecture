using Application.Helper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Application.Features.CustomerFeatures.Commands.RegisterCommand
{
    public class RegisterCommand : IRequest<RegisterCommandViewModel>
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandViewModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IConfiguration _config;

            public RegisterCommandHandler(ApplicationDbContext context, IConfiguration config)
            {
                _context = context;
                _config = config;
            }

            public async Task<RegisterCommandViewModel> Handle(RegisterCommand command, CancellationToken token)
            {
                command.Username = command.Username.ToLower();
                if (_context.Users.Any(u => u.Username == command.Username))
                {
                    return new RegisterCommandViewModel
                    {
                        IsSuccess = false,
                        Message = "Register failed"
                    };
                }
                var newUser = new User
                {
                    Username = command.Username,
                    Phone = command.Phone,
                    Address = command.Address,
                    Email = command.Email,
                    PasswordHash = EnCodeHelper.EnCodeSha1(command.Password),
                    CreatedDate = DateTime.UtcNow.Date,
                    ResetToken=""
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                var validToken = CreateToken(newUser.Username, newUser.Phone);
                return new RegisterCommandViewModel
                {
                    IsSuccess = true,
                    Message = validToken
                };
            }

            public string CreateToken(string username, string phone)
            {
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.NameId, username),
                    new Claim(JwtRegisteredClaimNames.PhoneNumber, phone),
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