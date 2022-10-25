using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.RoleFeatures.Queries
{
    public class GetAllRolesQuery : IRequest<IEnumerable<GetAllRolesQueryVM>>
    {
        internal class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<GetAllRolesQueryVM>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllRolesQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetAllRolesQueryVM>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
            {
                var listRole = await (from r in _context.Roles
                                      select new GetAllRolesQueryVM
                                      {
                                          Id = r.Id,
                                          Name = r.Name,
                                          Description = r.Description
                                      }).ToListAsync();

                return listRole.AsReadOnly();
            }
        }
    }
}