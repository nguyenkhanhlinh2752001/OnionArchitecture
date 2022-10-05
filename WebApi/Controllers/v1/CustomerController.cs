﻿using Application.Features.CustomerFeatures.Commands;
using Application.Features.CustomerFeatures.Commands.ForgetPasswordCommand;
using Application.Features.CustomerFeatures.Commands.LoginCommand;
using Application.Features.CustomerFeatures.Commands.RegisterCommand;
using Application.Features.CustomerFeatures.Commands.UpdatePasswordCommand;
using Application.Features.CustomerFeatures.Queries;
using Application.Features.CustomerFeatures.Queries.GetCustomersByPhoneQuery;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class CustomerController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCustomerByIdCommand { Id = id }));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCustomersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetCustomerByIdQuery { Id = id }));
        }

        [HttpGet("{id}/Orders")]
        public async Task<IActionResult> GetAllOrdersByCustomerId(int id)
        {
            return Ok(await Mediator.Send(new GetAllOrdersByCustomerIdQuery { Id = id }));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            return Ok(await Mediator.Send(new GetCustomersByPhoneQuery { Phone = phone }));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromForm] ForgetPasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}