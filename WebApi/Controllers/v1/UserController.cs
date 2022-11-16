using Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery;
using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;

namespace WebApi.Controllers.v1
{
    public class UserController : BaseApiController
    {
        

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await UserService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await UserService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            var result = await UserService.ForgetPassword(model);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var result = await UserService.ResetPassword(model);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var result = await UserService.ChangePassword(model);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(UpdateInfoDTO model)
        {
            var result = await UserService.UpdateInfo(model);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersByUserId([FromQuery] GetOrdersByUserIdQueryParameter query)
        {
            return Ok(await Mediator.Send(new GetOrdersByUserIdQuery
            {
                TotalPrice = query.TotalPrice,
                CreatedOn = query.CreatedOn,
                OrderBy = query.OrderBy,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }));
        }
    }
}