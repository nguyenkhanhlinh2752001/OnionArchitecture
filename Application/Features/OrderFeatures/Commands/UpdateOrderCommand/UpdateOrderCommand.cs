using Application.Exceptions;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands.UpdateOrderCommand
{
    public class UpdateOrderCommand : IRequest<Response<int>>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;

            public UpdateOrderCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<int>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                var exists = await _context.OrderDetails.FirstOrDefaultAsync(p => p.OrderId == request.OrderId && p.ProductId == request.ProductId);
                if (exists == null)
                {
                    try
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);
                        if (product == null) throw new ApiException("Product not found");
                        if (product.Quantity > request.Quantity)
                        {
                            var orderDetail = new OrderDetail()
                            {
                                OrderId = request.OrderId,
                                ProductId = request.ProductId,
                                Quantity = request.Quantity,
                                CreatedOn = DateTime.Now,
                                IsDeleted = false,
                            };
                            _context.OrderDetails.Add(orderDetail);
                            await _context.SaveChangesAsync();
                            var updateOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId);
                            if (updateOrder == null) throw new ApiException("Order not found");
                            updateOrder.TotalPrice = updateOrder.TotalPrice + request.Quantity * product.Price;
                            product.Quantity = product.Quantity - request.Quantity;
                            await _context.SaveChangesAsync();
                            await dbContextTransaction.CommitAsync();
                            await dbContextTransaction.DisposeAsync();
                            return new Response<int>(orderDetail.Id);
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
                return new Response<int>(0);
            }
        }
    }
}
