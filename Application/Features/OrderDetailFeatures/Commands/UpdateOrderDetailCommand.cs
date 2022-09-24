using Application.Features.CustomerFeatures.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderDetailFeatures.Commands
{
    public class UpdateOrderDetailCommand: IRequest<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public class UpdateOrderDetaiCommandHandler : IRequestHandler<UpdateOrderDetailCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateOrderDetaiCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateOrderDetailCommand command, CancellationToken cancellationToken)
            {
                var obj = _context.OrderDetails.Where(p => p.OrderId == command.OrderId && p.ProductId == command.ProductId).FirstOrDefault();
                obj.Quantity = command.Quantity;
                await _context.SaveChangesAsync();

                UpdateTotalPrice(command.OrderId);
                await _context.SaveChangesAsync();

                return obj.Id;

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
        }
    }
}
