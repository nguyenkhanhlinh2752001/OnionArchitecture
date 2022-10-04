using Application.Features.ProductFeatures.Commands;
using Application.Features.ProductFeatures.Queries;
using Application.Features.ProductFeatures.Queries.GetProductsByNameQuery;
using Application.Features.ProductFeatures.Queries.GetProductsByPriceQuery;
using Application.Features.ProductFeatures.Queries.GetProductsSoldQuery;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class ProductController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSold()
        {
            return Ok(await Mediator.Send(new GetProductsSoldQuery()));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await Mediator.Send(new GetProductsByNameQuery { Name = name }));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByPrice(decimal from, decimal to)
        {
            return Ok(await Mediator.Send(new GetProductsByPriceQuery { FromPrice = from, ToPrice = to }));
        }
    }
}