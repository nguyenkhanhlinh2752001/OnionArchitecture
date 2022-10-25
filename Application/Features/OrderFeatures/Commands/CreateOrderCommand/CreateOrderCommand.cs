using Application.Exceptions;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.OrderFeatures.Commands.CreateOrderCommand
{
    public class CreateOrderCommand : IRequest<Response<int>>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal class CreateOrderCommandHanler : IRequestHandler<CreateOrderCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public CreateOrderCommandHanler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<Response<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);
                    if (product == null) throw new ApiException("Product not found");
                    if (product.Quantity > request.Quantity)
                    {
                        var order = new Order()
                        {
                            UserId = _currentUserService.Id,
                            CreatedOn = DateTime.Now,
                            IsDeleted = false
                        };
                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();
                        var orderDetail = new OrderDetail()
                        {
                            OrderId = order.Id,
                            ProductId = request.ProductId,
                            Quantity = request.Quantity,
                            CreatedOn = DateTime.Now,
                            IsDeleted = false,
                        };
                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                        //update order total price && product quantity
                        var updateOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
                        if (updateOrder == null) throw new ApiException("Order not found");
                        updateOrder.TotalPrice = updateOrder.TotalPrice + request.Quantity * product.Price;
                        product.Quantity = product.Quantity - request.Quantity;
                        await _context.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();
                        await dbContextTransaction.DisposeAsync();
                        return new Response<int>(order.Id);
                    }
                    else
                        return new Response<int>(0);
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                    await dbContextTransaction.DisposeAsync();
                    throw;
                }
            }
        }
    }
}