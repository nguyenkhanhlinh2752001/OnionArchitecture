using Application.Features.ProductFeatures.Commands.CreateProduct;
using Application.Features.ProductFeatures.Commands.DeleteProductById;
using Application.Features.ProductFeatures.Commands.UpdateProduct;
using Application.Features.ProductFeatures.Queries.GetAllProducts;
using Application.Features.ProductFeatures.Queries.GetProductById;
using Application.Features.ProductFeatures.Queries.GetProductsSoldQuery;
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

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Update)]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("sold")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Access)]
        public async Task<IActionResult> GetSold()
        {
            return Ok(await Mediator.Send(new GetProductsSoldQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsParameter query)
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery
            {
                ProductName = query.ProductName,
                CategoryName = query.CategoryName,
                FromPrice = query.FromPrice,
                ToPrice = query.ToPrice,
                FromRate=query.FromRate,
                ToRate=query.ToRate,
                Order = query.Order,
                SortBy = query.SortBy,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }));
        }
    }
}