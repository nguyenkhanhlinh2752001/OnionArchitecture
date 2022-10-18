using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;

namespace WebApi.Controllers.v1
{
    public class UserController : BaseApiController
    {
        [HttpGet("{id}/Orders")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllOrdersByCustomerId(string id)
        {
            return Ok(await Mediator.Send(new GetAllOrdersByCustomerIdQuery { Id = id }));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await UserService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await UserService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            var result = await UserService.ForgetPassword(model);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var result = await UserService.ResetPassword(model);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var result = await UserService.ChangePassword(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(UpdateInfoDTO model)
        {
            var result = await UserService.UpdateInfo(model);
            return Ok(result);
        }
    }
}