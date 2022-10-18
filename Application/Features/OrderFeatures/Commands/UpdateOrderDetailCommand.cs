using Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands
{
    public class UpdateOrderDetailCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public UpdateOrderDetailCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateOrderDetailCommand command, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderId == command.OrderId && od.ProductId == command.ProductId);
                    if (orderDetail == null) throw new ApiException("Order detail not found");
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
                    if (product == null) throw new ApiException("Product not found");
                    var oldQuantity = orderDetail.Quantity;
                    var oldUnitPrice = orderDetail.Quantity * product.Price;
                    orderDetail.Quantity = command.Quantity;
                    await _context.SaveChangesAsync();
                    //update order total price and product quantity
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == command.OrderId);
                    if (order == null) throw new ApiException("Order not found");
                    product.Quantity = product.Quantity + oldQuantity - command.Quantity;
                    order.TotalPrice = order.TotalPrice - oldUnitPrice + product.Price * command.Quantity;
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    await dbContextTransaction.DisposeAsync();
                    return order.Id;
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