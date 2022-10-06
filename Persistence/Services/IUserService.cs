using Persistence.Models;

namespace Persistence.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterModel model);
    }
}