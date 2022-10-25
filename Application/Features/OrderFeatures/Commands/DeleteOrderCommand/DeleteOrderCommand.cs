using Application.Exceptions;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands.DeleteOrderCommand
{
    public class DeleteOrderCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        internal class DeleteOrderCommandHanlder : IRequestHandler<DeleteOrderCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;

            public DeleteOrderCommandHanlder(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<int>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.Id);
                    if (order == null) throw new ApiException("Order not found");
                    var orderDetails = _context.OrderDetails.Where(od => od.OrderId == request.Id).ToList();
                    foreach (var orderDetail in orderDetails)
                    {
                        orderDetail.IsDeleted = true;
                        orderDetail.DeleledOn = DateTime.Now;
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
                        if (product == null) throw new ApiException("Product not found");
                        product.Quantity += orderDetail.Quantity;
                    }
                    order.IsDeleted = true;
                    order.DeleledOn = DateTime.Now;
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