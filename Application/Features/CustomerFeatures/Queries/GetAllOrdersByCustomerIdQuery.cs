using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Queries
{
    public class GetAllOrdersByCustomerIdQuery : IRequest<IEnumerable<OrderDTO>>
    {
        public int Id { get; set; }

        public class GetAllOrdersByCustomerIdQueryHandler : IRequestHandler<GetAllOrdersByCustomerIdQuery, IEnumerable<OrderDTO>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllOrdersByCustomerIdQueryHandler(ApplicationDbContext context)
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
                                      CreatedDate = o.CreatedDate,
                                      OrderDetails = (from p in _context.Products
                                                      join od in _context.OrderDetails
                                                      on p.Id equals od.ProductId
                                                      where od.OrderId == o.Id
                                                      select new OrderDetailDTO
                                                      {
                                                          ProductName = p.Name,
                                                          Price = p.Price,
                                                          Quantity = od.Quantity,
                                                          UnitPrice = p.Price * od.Quantity
                                                      }).ToList()
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}