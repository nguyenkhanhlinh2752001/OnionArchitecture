using Application.Features.CategoryFeatures.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<User>>
    {
        public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<User>>
        {
            private readonly ApplicationDbContext _context;
            public GetAllCustomersQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<User>> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken)
            {
                var list = await _context.Users.Where(p => p.IsActive == false).ToListAsync();
                if (list == null)
                {
                    return null;
                }
                return list.AsReadOnly();
            }
        }
    }
}
