using Application.Features.UserFeatures.Queries.GetAllUsersQuery;
using Application.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using Persistence.DTOs;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : BaseApiController
    {
        [HttpGet("AdminPage")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Access)]
        public async Task<IActionResult> PostSecuredData()
        {
            return Ok("This Secured Data is available only for Administrator.");
        }

        [HttpPost("AddRole")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Add)]
        public async Task<IActionResult> AddRoleAsync(AddRoleDTO model)
        {
            var result = await UserService.AddRoleAsync(model);
            return Ok(result);
        }

        [HttpPost("CreateUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Add)]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            var result = await UserService.CreateUser(model);
            return Ok(result);
        }

        [HttpGet("User")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Access)]
        public async Task<IActionResult> GetAll(string? fullname, string? phone, string? address, bool? active, DateTime? from, DateTime? to, string? order, string? sortBy, [FromQuery] PaginationFilter filter)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery { FullName = fullname, PhoneNumber = phone, Address = address, IsActive = active, CreatedFrom = from, CreatedTo = to, Order = order, SortBy = sortBy, Filter = filter }));
        }



        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Update)]
        public async Task<IActionResult> Update([FromQuery] UpdateUserDTO model)
        {
            var result = await UserService.UpdateUser(model);
            return Ok(result);
        }
    }
}