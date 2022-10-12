using Application.Features.MenuFeatures.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class MenuController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int id, UpdateMenuCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteMenuByIdCommand { Id = id }));
        }
    }
}