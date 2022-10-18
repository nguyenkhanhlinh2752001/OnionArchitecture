using Domain.Entities;
using MediatR;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }

        internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public CreateCategoryCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
            {
                var category = new Category()
                {
                    Name = command.Name,
                    CreatedOn = DateTime.Now,
                    CreatedBy = _currentUserService.Id,
                    IsDeleted = false,
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category.Id;
            }
        }
    }
}