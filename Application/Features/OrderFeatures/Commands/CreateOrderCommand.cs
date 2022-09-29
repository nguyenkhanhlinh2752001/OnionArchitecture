using Domain.Entities;
using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
        {
            private readonly ApplicationDbContext _context;
            public CreateOrderCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var product = _context.Products.Where(p => p.Id == command.ProductId).FirstOrDefault();
                    if (product.Quantity > command.Quantity)
                    {
                        var order = new Order()
                        {
                            CustomerId = command.CustomerId,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false

                        };
                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();

                        var orderDetail = new OrderDetail()
                        {
                            OrderId = order.Id,
                            ProductId = command.ProductId,
                            Quantity = command.Quantity,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                        };
                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();

                        //update order total price && product quantity
                        var updateOrder = _context.Orders.Where(o => o.Id == order.Id).FirstOrDefault();
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
