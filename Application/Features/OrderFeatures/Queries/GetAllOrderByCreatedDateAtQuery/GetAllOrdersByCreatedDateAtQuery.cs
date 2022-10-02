using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Queries.GetAllOrderByCreatedDateAtQuery
{
    public class GetAllOrdersByCreatedDateAtQuery : IRequest<IEnumerable<GetAllOrdersByCreatedDateAtViewModel>>
    {
        public DateTime CreatedDate { get; set; }

        public class GetAllOrdersByCreatedDateAtQueryHandler : IRequestHandler<GetAllOrdersByCreatedDateAtQuery, IEnumerable<GetAllOrdersByCreatedDateAtViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllOrdersByCreatedDateAtQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetAllOrdersByCreatedDateAtViewModel>> Handle(GetAllOrdersByCreatedDateAtQuery query, CancellationToken token)
            {
                var list = await (from o in _context.Orders
                                  join c in _context.Customers
                                  on o.CustomerId equals c.Id
                                  where o.CreatedDate.Date == query.CreatedDate.Date
                                  select new GetAllOrdersByCreatedDateAtViewModel
                                  {
                                      CustomerName = c.Name,
                                      TotalPrice = o.TotalPrice,
                                      CreatedDate = o.CreatedDate
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}