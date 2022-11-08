using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Persistence.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string _username;
        private string _id;
        private string _role;

        public string Username
        {
            get
            {
                _username = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "";
                return _username;
            }
        }

        public string Id
        {
            get
            {
                _id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid) ?? "";
                return _id;
            }
        }

        public string Role
        {
            get
            {
                _role = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? "";
                return _role;
            }
        }
    }
}