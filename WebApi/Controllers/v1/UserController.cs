using Application.Features.CustomerFeatures.Queries;
using Application.Features.CustomerFeatures.Queries.GetUsersSearchQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using Persistence.DTOs;
using Persistence.Services;
using WebApi.Attributes;

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
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Add)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Search(string? fullname, string? address, string? phone, bool? active, DateTime? from, DateTime? to)
        {
            return Ok(await Mediator.Send(new GetUsersSearchQuery
            {
                FullName = fullname,
                Address = address,
                PhoneNumber = phone,
                IsActive = active,
                CreatedFrom = from,
                CreatedTo = to
            }));
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
            var result = await UserService.ChangePassword(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(UpdateUserDTO model)
        {
            var result = await UserService.UpdateUser(model);
            return Ok(result);
        }
    }
}