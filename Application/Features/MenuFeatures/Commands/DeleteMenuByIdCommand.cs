using MediatR;
using Persistence.Context;

namespace Application.Features.MenuFeatures.Commands
{
    public class DeleteMenuByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuByIdCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public DeleteMenuCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(DeleteMenuByIdCommand command, CancellationToken token)
            {
                var obj = _context.Menus.FirstOrDefault(m => m.Id == command.Id);
                obj.IsDeleted = true;

                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }
    }
}