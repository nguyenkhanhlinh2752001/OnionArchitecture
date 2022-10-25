using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly UserManager<User> _userManager;

            public CreateProductCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
            {
                _context = context;
                _currentUserService = currentUserService;
                _userManager = userManager;
            }

            public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
            {
                var userId = _currentUserService.Id;
                var user = await _userManager.FindByIdAsync(userId);
                DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;            
                //Console.WriteLine(now.ToString("yyyyMMddHHmmssfff"));
                var product = new Product()
                {
                    CategoryId = command.CategoryId,
                    Barcode = now.ToString("yyyyMMddHHmmssfff"),
                    Name = command.Name,
                    Rate = command.Rate,
                    Description = command.Description,
                    Price = command.Price,
                    Quantity = command.Quantity,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user.FullName,
                    IsDeleted = false
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }
        }
    }
}