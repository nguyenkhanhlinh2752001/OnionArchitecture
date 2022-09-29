using MediatR;
using Persistence.Context;

namespace Application.Features.CategoryFeatures.Commands
{
    public class DeleteCategoryByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, int>
        {
            private readonly ApplicationDbContext _context;
            public DeleteCategoryByIdCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteCategoryByIdCommand command, CancellationToken cancellationToken)
            {
                var obj = _context.Categories.Where(a => a.Id == command.Id).FirstOrDefault();

                if (obj == null)
                {
                    return default;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.DeleledDate = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return obj.Id;
                }
            }
        }
    }
}
