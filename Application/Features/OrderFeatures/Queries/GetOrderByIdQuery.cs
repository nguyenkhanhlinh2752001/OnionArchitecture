using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderDTO>
    {
        public int Id { get; set; }

        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDTO>
        {
            private readonly ApplicationDbContext _context;

            public GetOrderByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<OrderDTO> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
            {
                var obj = await (from o in _context.Orders
                                 join c in _context.Customers on o.CustomerId equals c.Id
                                 where o.Id == query.Id && c.IsDeleted == false
                                 select new OrderDTO()
                                 {
                                     Id = o.Id,
                                     CustomerName = c.Name,
                                     TotalPrice = o.TotalPrice,
                                     CreatedDate = o.CreatedDate,
                                     OrderDetails = (from od in _context.OrderDetails
                                                     join p in _context.Products
                                                     on od.ProductId equals p.Id
                                                     where od.OrderId == o.Id
                                                     select new OrderDetailDTO
                                                     {
                                                         ProductName = p.Name,
                                                         Price = p.Price,
                                                         Quantity = od.Quantity,
                                                         UnitPrice = p.Price * od.Quantity,
                                                     }).ToList()
                                 }).FirstOrDefaultAsync();
                return obj;
            }
        }
    }
}