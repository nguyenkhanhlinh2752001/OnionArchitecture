using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands
{
    public class DeleteOrderDetailCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public class DeleteOrderDetailCommandHandler : IRequestHandler<DeleteOrderDetailCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public DeleteOrderDetailCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(DeleteOrderDetailCommand command, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var orderDetail = _context.OrderDetails.Where(od => od.OrderId == command.OrderId && od.ProductId == command.ProductId).FirstOrDefault();
                    orderDetail.IsDeleted = true;
                    orderDetail.DeleledOn = DateTime.Now;
                    await _context.SaveChangesAsync();

                    //update order total price and product quantity
                    var product = _context.Products.Where(p => p.Id == orderDetail.ProductId).FirstOrDefault();
                    var order = _context.Orders.Where(o => o.Id == command.OrderId).FirstOrDefault();
                    order.TotalPrice = order.TotalPrice - orderDetail.Quantity * product.Price;
                    product.Quantity = product.Quantity + orderDetail.Quantity;
                    await _context.SaveChangesAsync();

                    await dbContextTransaction.CommitAsync();
                    await dbContextTransaction.DisposeAsync();
                    return orderDetail.Id;
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