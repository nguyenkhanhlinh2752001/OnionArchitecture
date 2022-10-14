using Application.Features.OrderFeatures.Commands;
using Application.Features.OrderFeatures.Queries;
using Application.Features.OrderFeatures.Queries.GetAllOrderByCreatedDateAtQuery;
using Application.Features.OrderFeatures.Queries.GetAllOrdersByCreatedDateFromToQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Constants;
using Persistence.Services;
using WebApi.Attributes;

namespace WebApi.Controllers.v1
{
	public class OrderController : BaseApiController
	{
		private readonly ICurrentUserService _currentUserService;

		public OrderController(ICurrentUserService currentUserService)
		{
			_currentUserService = currentUserService;
		}

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
		public async Task<IActionResult> GetAll()
		{
			return Ok(await Mediator.Send(new GetAllOrdersQuery()));
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetByCreatedDate(DateTime date)
		{
			return Ok(await Mediator.Send(new GetAllOrdersByCreatedDateAtQuery { CreatedDate = date.Date }));
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetByCreatedDateFromTo(DateTime datefrom, DateTime dateto)
		{
			return Ok(await Mediator.Send(new GetAllOrdersByCreatedDateFromToQuery { CreatedDateFrom = datefrom.Date, CreatedDateTo = dateto.Date }));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(await Mediator.Send(new GetOrderByIdQuery { Id = id }));
		}
	}
}