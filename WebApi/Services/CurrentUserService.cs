using Application.Services;
using System.Security.Claims;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string _userName;
        private string _roleId;

        public string Username
        {
            get
            {
                _userName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                return _userName;
            }
        }

        public string RoleId
        {
            get
            {
                _roleId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? "";
                return _roleId;
            }
        }
    }
}