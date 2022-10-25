using Application.Exceptions;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<User> _userManager;
            private readonly ICurrentUserService _currentUserService;

            public UpdateCategoryCommandHandler(ApplicationDbContext context, UserManager<User> userManager, ICurrentUserService currentUserService)
            {
                _context = context;
                _userManager = userManager;
                _currentUserService = currentUserService;
            }

            public async Task<Response<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var id = _currentUserService.Id;
                var user = await _userManager.FindByIdAsync(id);
                var category = _context.Categories.Where(a => a.Id == request.Id).FirstOrDefault();
                if (category == null) throw new ApiException("Category not found");
                category.Name = request.Name;
                category.LastEditBy = user.FullName;
                await _context.SaveChangesAsync();
                return new Response<int>(category.Id);
            }
        }
    }
}