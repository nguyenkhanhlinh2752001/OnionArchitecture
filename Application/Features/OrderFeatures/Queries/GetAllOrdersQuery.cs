using Application.Features.CustomerFeatures.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeatures.Queries
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>
    {
        public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
        {
            private readonly ApplicationDbContext _context;
            public GetAllOrdersQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
            {
                var list = await _context.Orders.Where(p => p.IsDeleted == false).ToListAsync();
                if (list == null)
                {
                    return null;
                }
                return list.AsReadOnly();
            }
        }
    }
}
