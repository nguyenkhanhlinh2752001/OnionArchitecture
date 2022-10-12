using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.RoleFeatures.Commands.CreateMenuToRoleCommand
{
    public class CreateMenuToRoleCommand : IRequest<int>
    {
        public string RoleId { get; set; }
        public IEnumerable<CreateMenuToRoleCommandDTO> Menus { get; set; }

        public class CreateMenuToRoleCommandHandler : IRequestHandler<CreateMenuToRoleCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public CreateMenuToRoleCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateMenuToRoleCommand command, CancellationToken cancellationToken)
            {
                var listPer = await _context.Permissons.Where(p => p.RoleId.Equals(command.RoleId)).ToListAsync();
                _context.Permissons.RemoveRange(listPer);

                var per = command.Menus.Select(x => new Permission
                {
                    RoleId = command.RoleId,
                    MenuId = x.MenuId,
                    CanAccess = x.CanAccess,
                    CanAdd = x.CanAdd,
                    CanDelete = x.CanDelete,
                    CanUpdate = x.CanUpdate,
                });

                await _context.Permissons.AddRangeAsync(per);
                await _context.SaveChangesAsync();
                return 1;
            }
        }
    }
}