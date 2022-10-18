using Application.Exceptions;
using MediatR;
using Persistence.Context;

namespace Application.Features.CategoryFeatures.Commands
{
    public class UpdateCategoryCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
        {
            private readonly ApplicationDbContext _context;

            public UpdateCategoryCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var category = _context.Categories.Where(a => a.Id == command.Id).FirstOrDefault();
                if (category == null) throw new ApiException("Category not found");
                category.Name = command.Name;
                await _context.SaveChangesAsync();
                return category.Id;
            }
        }
    }
}