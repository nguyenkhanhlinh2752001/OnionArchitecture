using Application.Features.CartFeatures.Commands.AddEditCartCommand;
using Application.Features.CartFeatures.Commands.DeleteCartCommand;
using Application.Features.CartFeatures.Queries.GetCartByUserIdCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseApiController
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(AddEditCartCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(int id)
        {
            return Ok(await Mediator.Send(new DeleteCartCommand { Id = id }));
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetByUserId()
        {
            return Ok(await Mediator.Send(new GetCartByUserIdQuery()));
        }
    }
}