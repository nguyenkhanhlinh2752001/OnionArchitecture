using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Commands
{
    public class DeleteOrderCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public DeleteOrderCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
            {
                var dbContextTransaction = _context.Database.BeginTransaction();
                try
                {
                    var order = _context.Orders.Where(o => o.Id == command.Id).FirstOrDefault();
                    var orderDetails = _context.OrderDetails.Where(od => od.OrderId == command.Id).ToList();
                    foreach (var orderDetail in orderDetails)
                    {
                        orderDetail.IsDeleted = true;
                        orderDetail.DeleledOn = DateTime.Now;
                        var product = _context.Products.FirstOrDefault(p => p.Id == orderDetail.ProductId);
                        product.Quantity += orderDetail.Quantity;
                    }
                    order.IsDeleted = true;
                    order.DeleledOn = DateTime.Now;
                    await _context.SaveChangesAsync();

                    dbContextTransaction.CommitAsync();
                    dbContextTransaction.DisposeAsync();

                    return order.Id;
                }
                catch (Exception)
                {
                    dbContextTransaction.RollbackAsync();
                    dbContextTransaction.DisposeAsync();
                    throw;
                }
            }
        }
    }
}