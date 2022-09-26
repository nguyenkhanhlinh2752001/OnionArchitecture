using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.OrderFeatures.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderDTO>
    {
        public int Id { get; set; }
        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDTO>
        {
            private readonly IApplicationDbContext _context;
            public GetOrderByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<OrderDTO> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
            {
                var obj =  (from o in _context.Orders
                             join c in _context.Customers on o.CustomerId equals c.Id
                             where o.Id == query.Id && c.IsDeleted==false 
                             select new OrderDTO()
                             {
                                 Id = o.Id,
                                 CustomerName = c.Name,
                                 TotalPrice = o.TotalPrice,
                                 CreatedDate=o.CreatedDate
                             }).FirstOrDefault();
                return obj;
            }
        }
    
    }
}
