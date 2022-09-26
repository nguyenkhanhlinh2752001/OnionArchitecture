using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Queries
{
    public class GetAllOrdersByCustomerIdQuery : IRequest<IEnumerable<OrderDTO>>
    {
        public int Id { get; set; }
        public class GetAllOrdersByCustomerIdQueryHandler : IRequestHandler<GetAllOrdersByCustomerIdQuery, IEnumerable<OrderDTO>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllOrdersByCustomerIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<OrderDTO>> Handle(GetAllOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
            {
                var list = await (from o in _context.Orders
                                  join c in _context.Customers on o.CustomerId equals c.Id
                                  where c.Id == query.Id && c.IsDeleted == false
                                  select new OrderDTO()
                                  {
                                      Id = o.Id,
                                      CustomerName = c.Name,
                                      TotalPrice = o.TotalPrice,
                                      CreatedDate = o.CreatedDate
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}
