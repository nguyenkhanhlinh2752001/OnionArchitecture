﻿using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Services;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpPost("welcome")]
        //public async Task<IActionResult> SendWelcomeMail([FromForm] WelcomeRequest request)
        //{
        //    try
        //    {
        //        await _mailService.SendWelcomeEmailAsync(request);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}