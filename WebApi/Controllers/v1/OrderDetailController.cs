using Application.Features.CustomerFeatures.Commands;
using Application.Features.OrderDetailFeatures.Commands;
using Application.Features.OrderFeatures.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class OrderDetailController: BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDetailCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int orderId, int productId, UpdateOrderDetailCommand command)
        {
            if (orderId != command.OrderId || productId!=command.ProductId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> Delete(int orderId, int productId)
        {
            return Ok(await Mediator.Send(new DeleteOrderDetailByIdCommand { OrderId = orderId, ProductId=productId }));
        }
    }
}
