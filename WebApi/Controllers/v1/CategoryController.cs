using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Queries;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class CategoryController : BaseApiController
    {
        private readonly ICurrentUserService _currentUserService;

        public CategoryController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryByIdCommand { Id = id }));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            //var tmp = _currentUserService.Username;
            //var tmp2 = _currentUserService.RoleId;
            return Ok(await Mediator.Send(new GetAllCategoriesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsByCategoryId(int id)
        {
            return Ok(await Mediator.Send(new GetProductsByCategoryIdQuery { Id = id }));
        }
    }
}