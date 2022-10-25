using Application.Exceptions;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands.UpdateOrderDetailCommand
{
    public class UpdateOrderDetailCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        internal class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;

            public UpdateOrderDetailCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<int>> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.Id == request.Id);
                    if (orderDetail == null) throw new ApiException("Order detail not found");
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
                    if (product == null) throw new ApiException("Product not found");
                    var oldQuantity = orderDetail.Quantity;
                    var oldUnitPrice = orderDetail.Quantity * product.Price;
                    orderDetail.Quantity = request.Quantity;
                    await _context.SaveChangesAsync();
                    //update order total price and product quantity
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderDetail.OrderId);
                    if (order == null) throw new ApiException("Order not found");
                    product.Quantity = product.Quantity + oldQuantity - request.Quantity;
                    order.TotalPrice = order.TotalPrice - oldUnitPrice + product.Price * request.Quantity;
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    await dbContextTransaction.DisposeAsync();
                    return new Response<int>(order.Id);
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