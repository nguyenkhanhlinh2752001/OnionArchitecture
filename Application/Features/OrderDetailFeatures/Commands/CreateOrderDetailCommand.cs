using Application.Features.OrderFeatures.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.OrderDetailFeatures.Commands
{
    public class CreateOrderDetailCommand: IRequest<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public class CreateOrderDetailCommandHandler : IRequestHandler<CreateOrderDetailCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateOrderDetailCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateOrderDetailCommand command, CancellationToken cancellationToken)
            {
                var query = _context.OrderDetails.Where(p => p.OrderId == command.OrderId && p.ProductId == command.ProductId).Count();
                if (query == 0)
                {
                    var obj = new OrderDetail();
                    obj.OrderId = command.OrderId;
                    obj.ProductId = command.ProductId;
                    obj.Quantity = command.Quantity;
                    obj.CreatedDate = DateTime.Now;
                    obj.IsDeleted = false;

                    _context.OrderDetails.Add(obj);
                    await _context.SaveChangesAsync();

                    UpdateTotalPrice(command.OrderId);
                    await _context.SaveChangesAsync();
                    return obj.Id;
                }
                else return 0;
                
            }
            public async void UpdateTotalPrice(int orderId)
            {
                decimal y = 0;
                var detailList = _context.OrderDetails.Where(p => p.OrderId == orderId && p.IsDeleted == false).ToList();
                foreach(var detail in detailList)
                {
                    decimal x = 0;
                    var product = _context.Products.Where(p => p.Id == detail.ProductId).FirstOrDefault();
                    x = product.Price * detail.Quantity;
                    y += x;
                    
                }
                var order = _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
                order.TotalPrice = y;
            }
        }
    }
}
