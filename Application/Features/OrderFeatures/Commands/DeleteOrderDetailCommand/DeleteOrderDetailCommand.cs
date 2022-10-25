using Application.Exceptions;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands.DeleteOrderDetailCommand
{
    public class DeleteOrderDetailCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        internal class DeleteOrderDetailCommandHandler : IRequestHandler<DeleteOrderDetailCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;

            public DeleteOrderDetailCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<int>> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var orderDetail = _context.OrderDetails.Where(od => od.Id == request.Id).FirstOrDefault();
                    if (orderDetail == null) throw new ApiException("Order detail not found");
                    orderDetail.IsDeleted = true;
                    orderDetail.DeleledOn = DateTime.Now;
                    await _context.SaveChangesAsync();
                    //update order total price and product quantity
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
                    if (product == null) throw new ApiException("Product not found");
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderDetail.OrderId);
                    if (order == null) throw new ApiException("Order not found");
                    order.TotalPrice = order.TotalPrice - orderDetail.Quantity * product.Price;
                    product.Quantity = product.Quantity + orderDetail.Quantity;
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    await dbContextTransaction.DisposeAsync();
                    return new Response<int>(orderDetail.Id);
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