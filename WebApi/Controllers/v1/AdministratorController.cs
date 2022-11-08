using Application.Features.OrderFeatures.Queries.GetAllOrdersQuery;
using Application.Features.UserFeatures.Queries.GetAllUsersQuery;
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
        /// <summary>
        /// Add Role To User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("role")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Add)]
        public async Task<IActionResult> AddRoleAsync(AddRoleDTO model)
        {
            var result = await UserService.AddRoleAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Administrator Create New User with role admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Add)]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            var result = await UserService.CreateUser(model);
            return Ok(result);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Access)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersParameter query)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery
            {
                FullName = query.FullName,
                PhoneNumber = query.PhoneNumber,
                Address = query.Address,
                IsActive = query.IsActive,
                CreatedFrom = query.CreatedFrom,
                CreatedTo = query.CreatedTo,
                Order = query.Order,
                SortBy = query.SortBy,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }));
        }

        /// <summary>
        /// Admin update user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.UserPermission, ConstantsAtr.Update)]
        public async Task<IActionResult> Update(UpdateUserDTO model)
        {
            var result = await UserService.UpdateUser(model);
            return Ok(result);
        }

        
    }
}