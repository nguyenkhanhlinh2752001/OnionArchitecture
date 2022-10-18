using Application.Features.OrderFeatures.Commands;
using Application.Features.OrderFeatures.Queries;
using Application.Features.OrderFeatures.Queries.GetAllOrdersQuery;
using Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery;
using Application.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
    public class OrderController : BaseApiController
    {
        [HttpPost]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Add)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Update)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Update)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateOrderDetail(UpdateOrderDetailCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("[action]")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Delete)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteOrderDetail(DeleteOrderDetailCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("[action]")]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Delete)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteOrder(DeleteOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [CustomAuthorizeAtrtibute(ConstantsAtr.OrderPermission, ConstantsAtr.Access)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll(string? name, string? phone, decimal? fromPrice, decimal? toPrice, DateTime? fromDate, DateTime? toDate, string? order, string? sortBy, [FromQuery] PaginationFilter filter)
        {
            return Ok(await Mediator.Send(new GetAllOrdersQuery { UserName = name, PhoneNumber = phone, TotalPriceFrom = fromPrice, TotalPriceTo = toPrice, CreatedFrom = fromDate, CreatedTo = toDate, Order = order, SortBy = sortBy, Filter = filter }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            return Ok(await Mediator.Send(new GetOrderByIdQuery { Id = id }));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByUser(decimal? fromPrice, decimal? toPrice, DateTime? fromDate, DateTime? toDate, string? order, string? sortBy, [FromQuery] PaginationFilter filter)
        {
            return Ok(await Mediator.Send(new GetOrdersByUserIdQuery { FromPrice = fromPrice, ToPrice = toPrice, FromDate = fromDate, ToDate = toDate, Order = order, SortBy = sortBy, Filter = filter }));
        }
    }
}