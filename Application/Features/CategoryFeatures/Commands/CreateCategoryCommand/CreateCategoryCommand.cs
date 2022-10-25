using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands.CreateCategoryCommand
{
    public class CreateCategoryCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }

        internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly UserManager<User> _userManager;

            public CreateCategoryCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<Response<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var user = await _userManager.FindByIdAsync(userId);
                var category = new Category()
                {
                    Name = request.Name,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user.FullName,
                    IsDeleted = false,
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return (new Response<int>(category.Id));
            }
        }
    }
}