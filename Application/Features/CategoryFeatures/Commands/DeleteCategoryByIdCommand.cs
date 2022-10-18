using MediatR;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands
{
    public class DeleteCategoryByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public DeleteCategoryByIdCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
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
                    obj.DeleledOn = DateTime.Now;
                    obj.DeletedBy = _currentUserService.Id;

                    await _context.SaveChangesAsync();
                    return obj.Id;
                }
            }
        }
    }
}