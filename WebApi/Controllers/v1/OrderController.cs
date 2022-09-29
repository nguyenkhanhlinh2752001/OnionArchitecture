using Application.Features.OrderFeatures.Commands;
using Application.Features.OrderFeatures.Queries;
using Application.Features.OrderFeatures.Queries.GetAllOrderByCreatedDateAtQuery;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class OrderController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrderDetail(UpdateOrderDetailCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteOrderDetail(DeleteOrderDetailCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteOrder(DeleteOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllOrdersQuery()));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByCreatedDate(DateTime date)
        {
            return Ok(await Mediator.Send(new GetAllOrdersByCreatedDateAtQuery { CreatedDate=date.Date}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetOrderByIdQuery { Id = id }));
        }
    }
}