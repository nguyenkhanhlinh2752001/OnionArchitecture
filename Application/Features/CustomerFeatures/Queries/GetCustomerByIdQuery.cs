using Application.Features.CategoryFeatures.Queries;
using Domain.Entities;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Queries
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public int Id { get; set; }
        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
        {
            private readonly ApplicationDbContext _context;
            public GetCustomerByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Customer> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var obj = _context.Customers.Where(a => a.Id == query.Id && a.IsDeleted == false).FirstOrDefault();
                if (obj == null) return null;
                return obj;
            }
        }
    }
}
