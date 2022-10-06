using Application.Features.OrderFeatures.Queries.GetAllOrderByCreatedDateAtQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Queries.GetAllOrdersByCreatedDateFromToQuery
{
    public class GetAllOrdersByCreatedDateFromToQuery : IRequest<IEnumerable<GetAllOrdersByCreatedDateFromToViewModel>>

    {
        public DateTime CreatedDateFrom { get; set; }
        public DateTime CreatedDateTo { get; set; }

        public class GetAllOrdersByCreatedDateFromToQueryHandler : IRequestHandler<GetAllOrdersByCreatedDateFromToQuery, IEnumerable<GetAllOrdersByCreatedDateFromToViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllOrdersByCreatedDateFromToQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetAllOrdersByCreatedDateFromToViewModel>> Handle(GetAllOrdersByCreatedDateFromToQuery query, CancellationToken token)
            {
                var list = await (from o in _context.Orders
                                  join c in _context.Users
                                  on o.CustomerId equals c.Id
                                  where (o.CreatedDate.Date >= query.CreatedDateFrom.Date) && (o.CreatedDate.Date <= query.CreatedDateTo.Date)
                                  select new GetAllOrdersByCreatedDateFromToViewModel
                                  {
                                      CustomerName = c.UserName,
                                      TotalPrice = o.TotalPrice,
                                      CreatedDate = o.CreatedDate
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}