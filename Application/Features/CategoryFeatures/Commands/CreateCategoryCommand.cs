using Domain.Entities;
using MediatR;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }

        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
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
                var obj = new Category();
                obj.Name = command.Name;
                obj.CreatedOn = DateTime.Now;
                obj.CreatedBy = _currentUserService.Id;
                obj.IsDeleted = false;

                _context.Categories.Add(obj);
                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }
    }
}