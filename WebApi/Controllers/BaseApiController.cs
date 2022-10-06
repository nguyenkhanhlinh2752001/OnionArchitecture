using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        private IUserService _userService;

        protected IUserService UserService => _userService ??= HttpContext.RequestServices.GetService<IUserService>();
    }
}