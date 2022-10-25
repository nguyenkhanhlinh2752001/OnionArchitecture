using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.ProductFeatures.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        internal class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly UserManager<User> _userManager;

            public DeleteProductByIdCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var user = await _userManager.FindByIdAsync(userId);
                var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == command.Id);
                if (product == null) throw new ApiException("Product not found");
                product.IsDeleted = true;
                product.DeleledOn = DateTime.Now;
                product.DeletedBy = user.FullName;
                await _context.SaveChangesAsync();
                return product.Id;
            }
        }
    }
}