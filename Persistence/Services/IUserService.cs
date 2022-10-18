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

        Task<AuthenticationVM> ChangePassword(ChangePasswordDTO model);

        Task<string> CreateUser(CreateUserDTO model);

        Task<string> UpdateInfo(UpdateInfoDTO model);

        Task<string> UpdateUser(UpdateUserDTO model);

        Task<string> UnActiveUser(string id);

        Task<bool> CheckPermisson(string funcUrl, string action, string role);
    }
}