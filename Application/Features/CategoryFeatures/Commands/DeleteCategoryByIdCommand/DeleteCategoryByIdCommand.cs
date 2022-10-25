using Application.Exceptions;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.CategoryFeatures.Commands.DeleteCategoryByIdCommand
{
    public class DeleteCategoryByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        internal class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, Response<int>>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly UserManager<User> _userManager;

            public DeleteCategoryByIdCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<Response<int>> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                var id = _currentUserService.Id;
                var user = await _userManager.FindByIdAsync(id);
                var category = _context.Categories.Where(a => a.Id == request.Id).FirstOrDefault();
                if (category == null) throw new ApiException("Category not found");
                category.IsDeleted = true;
                category.DeleledOn = DateTime.Now;
                category.DeletedBy = user.FullName;
                await _context.SaveChangesAsync();
                return new Response<int>(category.Id);
            }
        }
    }
}