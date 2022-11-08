using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Constants;
using Persistence.Context;
using Persistence.DTOs;
using Persistence.Settings;
using Persistence.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JWT _jwt;
        private readonly IMailService _mailService;
        private readonly ICurrentUserService _currentUserService;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JWT> jwt, ApplicationDbContext context, IMailService mailService, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
            _mailService = mailService;
            _currentUserService = currentUserService;
        }

        public async Task<AuthenticationVM> LoginAsync(LoginDTO model)
        {
            var authenticationModel = new AuthenticationVM();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationModel.Message = $"{user.Email} login successfully ";
                authenticationModel.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address,
                CreatedOn = DateTime.Now.Date,
                EmailConfirmed = true,
                PhoneNumber = model.Phone
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.CustomerRole.ToString());
                    await _context.SaveChangesAsync();
                    var cart = new Cart
                    {
                        UserId = user.Id
                    };
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();
                }
                return $"User Registered successfully with username {user.UserName}";
            }
            else
            {
                return $"Email {user.Email} is already registered.";
            }
        }

        public async Task<string> AddRoleAsync(AddRoleDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return $"No Accounts Registered with {model.Email}.";
            }

            var roleExists = Enum.GetNames(typeof(RoleConstants.Roles)).Any(x => x.ToLower() == model.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(RoleConstants.Roles)).Cast<RoleConstants.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return $"Added {model.Role} to user {model.Email}.";
            }
            return $"Role {model.Role} not found.";
        }

        public async Task<AuthenticationVM> ForgetPassword(ForgetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var authenticationModel = new AuthenticationVM();
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
                return authenticationModel;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            user.RefreshToken = token;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_jwt.DurationInMinutes);
            await _context.SaveChangesAsync();
            if (await _mailService.ForgetPasswordSendMail(user.Email, user.UserName, token))
            {
                authenticationModel.Message = $"Check your email at {model.Email} to reset password";
                authenticationModel.IsAuthenticated = true;
                authenticationModel.RefreshToken = token;
                authenticationModel.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_jwt.DurationInMinutes);
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationModel;
        }

        public async Task<AuthenticationVM> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var authenticationModel = new AuthenticationVM();
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {user.Email}.";
                return authenticationModel;
            }
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (resetPassResult.Succeeded)
            {
                authenticationModel.Message = $"{user.UserName} reset password successfully";
                authenticationModel.IsAuthenticated = true;
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;
            }
            else
            {
                authenticationModel.Errors = resetPassResult.Errors.Select(e => e.Description);
                return authenticationModel;
            }
        }

        public async Task<AuthenticationVM> ChangePassword(ChangePasswordDTO model)
        {
            var id = _currentUserService.Id;
            var user = await _userManager.FindByIdAsync(id);
            var authenticationModel = new AuthenticationVM();
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {user.Email}.";
                return authenticationModel;
            }
            var rs = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (rs.Succeeded)
            {
                authenticationModel.Message = $"{user.UserName} changes password successfully";
                authenticationModel.IsAuthenticated = true;
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;
            }
            else
            {
                authenticationModel.Errors = rs.Errors.Select(e => e.Description);
                return authenticationModel;
            }
        }

        public async Task<string> CreateUser(CreateUserDTO model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address,
                CreatedOn = DateTime.Now.Date,
                EmailConfirmed = true,
                PhoneNumber = model.Phone
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.AdministratorRole.ToString());
                }
                return $"User created successfully with username {user.UserName}";
            }
            else
            {
                return $"Email {user.Email} is already registered.";
            }
        }

        public async Task<string> UpdateInfo(UpdateInfoDTO model)
        {
            var id = _currentUserService.Id;
            var user = await _userManager.FindByIdAsync(id);
            user.FullName = model.FullName;
            user.Address = model.Address;
            user.PhoneNumber = model.Phone;
            user.UserName = model.Username;
            await _context.SaveChangesAsync();
            return $"{user.UserName} updated successfully";
        }

        public async Task<string> UpdateUser(UpdateUserDTO model)
        {
            var id = model.Id;
            var user = await _userManager.FindByIdAsync(id);
            user.IsActive = model.IsActive;
            await _context.SaveChangesAsync();
            return $"{user.UserName} updated successfully";
        }

        public async Task<string> UnActiveUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.IsActive = false;
            await _context.SaveChangesAsync();
            return $"{user.UserName} inactive successfully";
        }

        public async Task<bool> CheckPermisson(string funcUrl, string action, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;
            var query = from p in _context.Permissons
                        join m in _context.Menus
                        on p.MenuId equals m.Id
                        where p.RoleId == role.Id
                        && m.Url == funcUrl &&
                        ((p.CanAccess && action == ConstantsAtr.Access)
                        || (p.CanAdd && action == ConstantsAtr.Add)
                        || (p.CanUpdate && action == ConstantsAtr.Update)
                        || (p.CanDelete && action == ConstantsAtr.Delete))
                        select p;
            return query.Any();
        }
    }
}