using Domain.Entities;
using MediatR;
using Persistence.Context;

namespace Application.Features.MenuFeatures.Commands
{
    public class CreateMenuCommand : IRequest<int>
    {
        public string Url { get; set; }

        public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public CreateMenuCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateMenuCommand command, CancellationToken token)
            {
                var obj = new Menu();
                obj.Url = command.Url;

                _context.Menus.Add(obj);
                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }
    }
}