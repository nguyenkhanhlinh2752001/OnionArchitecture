using Application.Features.CustomerFeatures.Commands;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderDetailFeatures.Commands
{
    public class DeleteOrderDetailByIdCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public class DeleteOrderDetailByIdCommandHandler : IRequestHandler<DeleteOrderDetailByIdCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public DeleteOrderDetailByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteOrderDetailByIdCommand command, CancellationToken cancellationToken)
            {
                var obj = _context.OrderDetails.Where(a => a.OrderId == command.OrderId && a.ProductId==command.ProductId).FirstOrDefault();

                if (obj == null)
                {
                    return default;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.DeleledDate = DateTime.Now;
                    await _context.SaveChangesAsync();

                    UpdateTotalPrice(command.OrderId);
                    await _context.SaveChangesAsync();

                    UpdateProductQuantity(command.ProductId, obj.Quantity);
                    await _context.SaveChangesAsync();

                    return obj.Id;
                }
            }
            public async void UpdateTotalPrice(int orderId)
            {
                decimal y = 0;
                var detailList = _context.OrderDetails.Where(p => p.OrderId == orderId && p.IsDeleted == false).ToList();
                int number = detailList.Count();
                foreach (var detail in detailList)
                {
                    decimal x = 0;
                    var product = _context.Products.Where(p => p.Id == detail.ProductId).FirstOrDefault();
                    x = product.Price * detail.Quantity;
                    y += x;

                }
                var order = _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
                order.TotalPrice = y;
            }

            public async void UpdateProductQuantity(int producId, int quantity)
            {
                var product = _context.Products.Where(p => p.Id == producId).FirstOrDefault();
                product.Quantity = product.Quantity + quantity;
            }
        }
    }
}
