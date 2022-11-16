using Application.Features.OrderFeatures.Commands.AddEditOrderCommand;
using Application.Features.OrderFeatures.Commands.DeleteOrderCommand;
using Application.Features.OrderFeatures.Commands.DeleteOrderDetailCommand;
using Application.Features.OrderFeatures.Queries.GetAllOrdersQuery;
using Application.Features.OrderFeatures.Queries.GetOrdersByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
    public class OrderController : BaseApiController
    {
        /// <summary>
        /// crrate an order
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Add)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(AddEditOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// delete order detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("order-detail/{id}")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Delete)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            return Ok(await Mediator.Send(new DeleteOrderDetailCommand { Id = id }));
        }

        /// <summary>
        /// delete order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Delete)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return Ok(await Mediator.Send(new DeleteOrderCommand { Id = id }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return Ok(await Mediator.Send(new GetOrdersByIdQuery { OrderId = id }));
        }

        [HttpGet]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Access)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersParameter query)
        {
            return Ok(await Mediator.Send(new GetAllOrdersQuery
            {
                UserName = query.UserName,
                PhoneNumber = query.PhoneNumber,
                TotalPrice = query.TotalPrice,
                OrderBy = query.OrderBy,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }));
        }
    }
}