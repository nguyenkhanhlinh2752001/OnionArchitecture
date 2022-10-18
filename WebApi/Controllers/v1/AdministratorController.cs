using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : BaseApiController
    {
        [HttpGet("AdminPage")]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostSecuredData()
        {
            return Ok("This Secured Data is available only for Administrator.");
        }

        [HttpPost("AddRole")]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddRoleAsync(AddRoleDTO model)
        {
            var result = await UserService.AddRoleAsync(model);
            return Ok(result);
        }

        [HttpPost("CreateUser")]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            var result = await UserService.CreateUser(model);
            return Ok(result);
        }

        [HttpGet("User")]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await UserService.GetAllUsers();
            return Ok(result);
        }

        [HttpPut("UnActiveUser")]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UnActiveUser(string email)
        {
            var result = await UserService.UnActiveUser(email);
            return Ok(result);
        }
    }
}