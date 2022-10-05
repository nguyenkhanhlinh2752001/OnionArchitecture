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

            public RegisterCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<RegisterCommandViewModel> Handle(RegisterCommand command, CancellationToken token)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
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
                        ResetToken = ""
                    };
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    var userRole = new UserRole
                    {
                        RoleId = 3,
                        UserId = newUser.Id,
                    };
                    _context.UserRoles.Add(userRole);
                    await _context.SaveChangesAsync();

                    await dbContextTransaction.CommitAsync();
                    await dbContextTransaction.DisposeAsync();

                    var validToken = TokenHelper.GenerateToken(newUser.Username,userRole.RoleId);
                    return new RegisterCommandViewModel
                    {
                        IsSuccess = true,
                        Message = validToken
                    };
                }
                catch(Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                    await dbContextTransaction.DisposeAsync();
                    throw;
                }
            }

            
        }
    }
}