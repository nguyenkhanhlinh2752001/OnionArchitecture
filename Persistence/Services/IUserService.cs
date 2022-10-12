using Persistence.DTOs;
using Persistence.ViewModels;

namespace Persistence.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDTO model);

        Task<AuthenticationVM> LoginAsync(LoginDTO model);

        Task<string> AddRoleAsync(AddRoleDTO model);

        Task<AuthenticationVM> ForgetPassword(ForgetPasswordDTO model);

        Task<AuthenticationVM> ResetPassword(ResetPasswordDTO model);

        Task<AuthenticationVM> ChangePassword(string email, ChangePasswordDTO model);

        Task<string> CreateUser(CreateUserDTO model);

        Task<IEnumerable<GetAllUsersVM>> GetAllUsers();

        Task<string> UpdateUser(string email, UpdateUserDTO model);

        Task<string> UnActiveUser(string id);

        Task<bool> CheckPermisson(string funcUrl, string action, string role);
    }
}