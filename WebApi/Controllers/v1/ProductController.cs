using Application.Features.ProductFeatures.Commands;
using Application.Features.ProductFeatures.Queries;
using Application.Features.ProductFeatures.Queries.GetProductsSoldQuery;
using Application.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
    public class ProductController : BaseApiController
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Add)]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }

        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Update)]
        public async Task<IActionResult> Update(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Access)]
        public async Task<IActionResult> GetSold()
        {
            return Ok(await Mediator.Send(new GetProductsSoldQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? productName, string? categoryName, decimal? from, decimal? to, string? order, string? sortBy, [FromQuery] PaginationFilter filter)
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery { ProductName = productName, CategoryName = categoryName, FromPrice = from, ToPrice = to, Order = order, SortBy = sortBy, Filter = filter }));
        }
    }
}