using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAtrtibute : Attribute, IAsyncAuthorizationFilter
    {
        private string Function { get; }
        private string Action { get; }

        public CustomAuthorizeAtrtibute(string function, string action)
        {
            Function = function;
            Action = action;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = "";
            var role = "";
            var tokenString = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(tokenString))
            {
                var jwtEncodedString = tokenString.Replace("Bearer ", "");
                var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
                userId = token.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
                role = token.Claims.First(c => c.Type == "roles").Value;
            }
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var user = userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new UnauthorizeException("Unauthorized");
            if (!string.IsNullOrEmpty(Function) && !string.IsNullOrEmpty(Action))
            {
                var permissionService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                if (!await permissionService.CheckPermisson(Function, Action, role))
                    throw new UnauthorizeException("Unauthorized");
            }
        }
    }
}