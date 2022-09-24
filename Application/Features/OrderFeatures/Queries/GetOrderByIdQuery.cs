using Application.Features.CustomerFeatures.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeatures.Queries
{
    public class GetOrderByIdQuery : IRequest<Order>
    {
        public int Id { get; set; }
        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
        {
            private readonly IApplicationDbContext _context;
            public GetOrderByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Order> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
            {
                var obj = _context.Orders.Where(a => a.Id == query.Id && a.IsDeleted == false).FirstOrDefault();
                if (obj == null) return null;
                return obj;
            }
        }
    
    }
}
