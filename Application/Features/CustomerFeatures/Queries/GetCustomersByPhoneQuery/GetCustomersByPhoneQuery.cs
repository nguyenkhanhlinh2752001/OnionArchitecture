using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Queries.GetCustomersByPhoneQuery
{
    public class GetCustomersByPhoneQuery : IRequest<IEnumerable<GetCustomerByPhoneViewModel>>
    {
        public string Phone { get; set; }

        public class GetCustomersByPhoneQueryHandler : IRequestHandler<GetCustomersByPhoneQuery, IEnumerable<GetCustomerByPhoneViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetCustomersByPhoneQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetCustomerByPhoneViewModel>> Handle(GetCustomersByPhoneQuery query, CancellationToken token)
            {
                var list = await (from c in _context.Customers
                                  where c.Phone.Contains(query.Phone)
                                  select new GetCustomerByPhoneViewModel
                                  {
                                      Name = c.Name,
                                      Phone = c.Phone,
                                      Address = c.Address,
                                  }).ToListAsync();
                return list.AsReadOnly();
            }
        }
    }
}