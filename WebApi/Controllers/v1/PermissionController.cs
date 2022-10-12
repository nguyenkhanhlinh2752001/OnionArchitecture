using Application.Features.RoleFeatures.Commands.CreateMenuToRoleCommand;
using Application.Features.RoleFeatures.Queries.GetAllMenusByRoleId;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenusByRoleId(string id)
        {
            return Ok(await Mediator.Send(new GetAllMenuByRoleIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuToRoleCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}