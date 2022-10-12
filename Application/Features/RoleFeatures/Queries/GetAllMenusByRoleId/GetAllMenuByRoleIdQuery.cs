using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.RoleFeatures.Queries.GetAllMenusByRoleId
{
    public class GetAllMenuByRoleIdQuery : IRequest<IEnumerable<GetAllMenusByRoleIdQueryVM>>
    {
        public string Id { get; set; }

        public class GetAllMenuByRoleIdQueryHandler : IRequestHandler<GetAllMenuByRoleIdQuery, IEnumerable<GetAllMenusByRoleIdQueryVM>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllMenuByRoleIdQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GetAllMenusByRoleIdQueryVM>> Handle(GetAllMenuByRoleIdQuery query, CancellationToken cancellationToken)
            {
                var t = await (from m in _context.Menus
                               join p in _context.Permissons
                               on m.Id equals p.MenuId into tem
                               from lf in tem.DefaultIfEmpty()
                               where lf.RoleId == query.Id
                               select new GetAllMenusByRoleIdQueryVM
                               {
                                   Name = m.Name,
                                   Url = m.Url,
                                   CanAccess = lf.CanAccess,
                                   CanAdd = lf.CanAdd,
                                   CanDelete = lf.CanDelete,
                                   CanUpdate = lf.CanUpdate
                               }).ToListAsync();
                return t;
            }
        }
    }
}