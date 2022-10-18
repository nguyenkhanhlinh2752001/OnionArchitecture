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
                                  join c in _context.Users
                                  on o.UserId equals c.Id
                                  where o.CreatedOn == query.CreatedDate.Date
                                  select new GetAllOrdersByCreatedDateAtViewModel
                                  {
                                      CustomerName = c.UserName,
                                      TotalPrice = o.TotalPrice,
                                      CreatedDate = o.CreatedOn
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}