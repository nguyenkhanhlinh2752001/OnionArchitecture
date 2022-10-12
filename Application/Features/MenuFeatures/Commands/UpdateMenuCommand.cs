using MediatR;
using Persistence.Context;

namespace Application.Features.MenuFeatures.Commands
{
    public class UpdateMenuCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public UpdateMenuCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateMenuCommand command, CancellationToken token)
            {
                var obj = _context.Menus.FirstOrDefault(m => m.Id == command.Id);
                if (obj == null)
                    return default;
                else
                {
                    obj.Url = command.Url;
                    await _context.SaveChangesAsync();
                    return obj.Id;
                }
            }
        }
    }
}