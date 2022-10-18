using Application.Exceptions;
using MediatR;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands
{
    public class DeleteCategoryByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        internal class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, int>
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
                var category = _context.Categories.Where(a => a.Id == command.Id).FirstOrDefault();
                if (category == null) throw new ApiException("Category not found");
                category.IsDeleted = true;
                category.DeleledOn = DateTime.Now;
                category.DeletedBy = _currentUserService.Id;
                await _context.SaveChangesAsync();
                return category.Id;
            }
        }
    }
}