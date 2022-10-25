using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly UserManager<User> _userManager;

            public UpdateProductCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var user = await _userManager.FindByIdAsync(userId);
                var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == command.Id);
                if (product == null) throw new ApiException("Product not found");
                product.CategoryId = command.CategoryId;
                product.Barcode = command.Barcode;
                product.Name = command.Name;
                product.Rate = command.Rate;
                product.Description = command.Description;
                product.Price = command.Price;
                product.Quantity = command.Quantity;
                product.LastEditBy = user.FullName;
                await _context.SaveChangesAsync();
                return product.Id;
            }
        }
    }
}