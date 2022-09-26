using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeatures.Queries
{
    public class GetAllOrderDetailsByIdOrderQuery : IRequest<IEnumerable<OrderDetailDTO>>
    {
        public int Id { get; set; }
        public class GetAllOrderDetailsByIdOrderQueryHandler : IRequestHandler<GetAllOrderDetailsByIdOrderQuery, IEnumerable<OrderDetailDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllOrderDetailsByIdOrderQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<OrderDetailDTO>> Handle(GetAllOrderDetailsByIdOrderQuery query, CancellationToken cancellationToken)
            {
                var list = await (from o in _context.Orders
                                  join d in _context.OrderDetails on o.Id equals d.OrderId
                                  join p in _context.Products on d.ProductId equals p.Id
                                  where o.Id == query.Id && d.IsDeleted == false
                                  select new OrderDetailDTO()
                                  {
                                      Id = d.Id,
                                      ProductName = p.Name,
                                      Price = p.Price,
                                      Quantity = d.Quantity,
                                      UnitPrice = p.Price * d.Quantity
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}
