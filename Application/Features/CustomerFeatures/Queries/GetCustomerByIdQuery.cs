using Domain.Entities;
using MediatR;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Queries
{
    public class GetCustomerByIdQuery : IRequest<User>
    {
        public int Id { get; set; }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, User>
        {
            private readonly ApplicationDbContext _context;

            public GetCustomerByIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<User> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var obj = _context.Users.Where(a => a.Id == query.Id && a.IsDeleted == false).FirstOrDefault();
                if (obj == null) return null;
                return obj;
            }
        }
    }
}