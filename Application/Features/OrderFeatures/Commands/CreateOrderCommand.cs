using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.OrderFeatures.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public CreateOrderCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == command.ProductId);
                    if (product == null) throw new ApiException("Product not found");
                    if (product.Quantity > command.Quantity)
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
                            ProductId = command.ProductId,
                            Quantity = command.Quantity,
                            CreatedOn = DateTime.Now,
                            IsDeleted = false,
                        };
                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                        //update order total price && product quantity
                        var updateOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
                        if (updateOrder == null) throw new ApiException("Order not found");
                        updateOrder.TotalPrice = updateOrder.TotalPrice + command.Quantity * product.Price;
                        product.Quantity = product.Quantity - command.Quantity;
                        await _context.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();
                        await dbContextTransaction.DisposeAsync();
                        return order.Id;
                    }
                    else
                        return 0;
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