using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Queries;
using Application.Features.CustomerFeatures.Commands;
using Application.Features.CustomerFeatures.Queries;
using Application.Features.OrderDetailFeatures.Commands;
using Application.Features.OrderFeatures.Commands;
using Application.Features.OrderFeatures.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class OrderController: BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllOrdersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetOrderByIdQuery { Id = id }));
        }




    }
}
