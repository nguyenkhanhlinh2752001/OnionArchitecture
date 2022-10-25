using Application.Features.PermissionFeatures.Commands.CreateMenuToRoleCommand;
using Application.Features.PermissionFeatures.Queries.GetAllMenusByRoleId;
using Application.Features.RoleFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : BaseApiController
    {
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.RolePermission, ConstantsAtr.Access)]
        public async Task<IActionResult> GetMenusByRoleId(string id)
        {
            return Ok(await Mediator.Send(new GetAllMenuByRoleIdQuery { Id = id }));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.RolePermission, ConstantsAtr.Add)]
        public async Task<IActionResult> Create(CreateMenuToRoleCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllRolesQuery()));
        }
    }
}