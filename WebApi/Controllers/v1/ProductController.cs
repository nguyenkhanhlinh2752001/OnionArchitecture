using Application.Features.ProductFeatures.Commands.AddEditProduct;
using Application.Features.ProductFeatures.Commands.DeleteProductById;
using Application.Features.ProductFeatures.Commands.DeleteProdutcDetailById;
using Application.Features.ProductFeatures.Queries.GetAllProducts;
using Application.Features.ProductFeatures.Queries.GetProductById;
using Application.Features.ProductFeatures.Queries.GetReviewsByProductId;
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
        public async Task<IActionResult> Create(AddEditProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { ProductId = id }));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }

        [HttpDelete("product-detail/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.ProductPermission, ConstantsAtr.Delete)]
        public async Task<IActionResult> DeleteProductDetail(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductDetailByIdCommand { Id = id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsParameter query)
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery
            {
                OrderBy = query.OrderBy,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                ProductName = query.ProductName
            }));
        }

        [HttpGet("review")]
        public async Task<IActionResult> GetReviewByProductId([FromQuery] GetReviewsByProductIdParametter query)
        {
            return Ok(await Mediator.Send(new GetReviewsByProductIdQuery
            {
                ProductId = query.ProductId,
                OrderBy = query.OrderBy,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Rate = query.Rate,
            }));
        }
    }
}