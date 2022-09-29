using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands
{
    public class UpdateOrderDetailCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }

        public class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, int>
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
                    var orderDetail = _context.OrderDetails.Where(od => od.Id == command.OrderDetailId && od.OrderId == command.OrderId).FirstOrDefault();
                    var product = _context.Products.Where(p => p.Id == orderDetail.ProductId).FirstOrDefault();

                    var oldQuantity = orderDetail.Quantity;
                    var oldUnitPrice = orderDetail.Quantity * product.Price;

                    orderDetail.Quantity = command.Quantity;
                    await _context.SaveChangesAsync();

                    //update order total price and product quantity
                    var order = _context.Orders.Where(o => o.Id == command.OrderId).FirstOrDefault();
                    product.Quantity = product.Quantity + oldQuantity - command.Quantity;
                    order.TotalPrice = order.TotalPrice - oldUnitPrice + product.Price * command.Quantity;
                    await _context.SaveChangesAsync();

                    dbContextTransaction.CommitAsync();
                    dbContextTransaction.DisposeAsync();

                    return order.Id;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}