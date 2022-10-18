using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.UserFeatures.Queries
{
    public class GetCustomerByIdQuery : IRequest<User>
    {
        public string Id { get; set; }

        internal class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, User>
        {
            private readonly ApplicationDbContext _context;

            public GetCustomerByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<User> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == query.Id);
                if (user == null) throw new ApiException("User not found");
                return user;
            }
        }
    }
}