using Application.Features.CustomerFeatures.Queries;
using Application.Features.CustomerFeatures.Queries.GetCustomersByPhoneQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;
using Persistence.Services;

namespace WebApi.Controllers.v1
{
    public class UserController : BaseApiController
    {
        private readonly ICurrentUserService _currentUserService;

        public UserController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateCustomerCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    return Ok(await Mediator.Send(new DeleteCustomerByIdCommand { Id = id }));
        //}

        //[HttpPut("[action]")]
        //public async Task<IActionResult> Update(string id, UpdateCustomerCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Ok(await Mediator.Send(new GetAllCustomersQuery()));
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(string id)
        //{
        //    return Ok(await Mediator.Send(new GetCustomerByIdQuery { Id = id }));
        //}

        [HttpGet("{id}/Orders")]
        public async Task<IActionResult> GetAllOrdersByCustomerId(string id)
        {
            return Ok(await Mediator.Send(new GetAllOrdersByCustomerIdQuery { Id = id }));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            return Ok(await Mediator.Send(new GetCustomersByPhoneQuery { Phone = phone }));
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
            var email = _currentUserService.Email;
            var id = _currentUserService.Id;
            var role = _currentUserService.Role;
            var result = await UserService.ChangePassword(email, model);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateUserDTO model)
        {
            var email = _currentUserService.Email;
            var result = await UserService.UpdateUser(email, model);
            return Ok(result);
        }
    }
}