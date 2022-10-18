using Domain.Entities;
using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands
{
    public class UpdateOrderCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public UpdateOrderCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                var exists = _context.OrderDetails.FirstOrDefault(p => p.OrderId == command.OrderId && p.ProductId == command.ProductId);
                if (exists == null)
                {
                    try
                    {
                        var product = _context.Products.Where(p => p.Id == command.ProductId).FirstOrDefault();
                        if (product.Quantity > command.Quantity)
                        {
                            var orderDetail = new OrderDetail()
                            {
                                OrderId = command.OrderId,
                                ProductId = command.ProductId,
                                Quantity = command.Quantity,
                                CreatedOn = DateTime.Now,
                                IsDeleted = false,
                            };
                            _context.OrderDetails.Add(orderDetail);
                            await _context.SaveChangesAsync();

                            var updateOrder = _context.Orders.Where(o => o.Id == command.OrderId).FirstOrDefault();
                            updateOrder.TotalPrice = updateOrder.TotalPrice + command.Quantity * product.Price;
                            product.Quantity = product.Quantity - command.Quantity;
                            await _context.SaveChangesAsync();

                            await dbContextTransaction.CommitAsync();
                            await dbContextTransaction.DisposeAsync();
                            return orderDetail.Id;
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
                return 0;
            }
        }
    }
}