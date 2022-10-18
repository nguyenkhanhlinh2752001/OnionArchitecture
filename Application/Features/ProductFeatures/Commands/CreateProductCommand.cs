using Domain.Entities;
using MediatR;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.ProductFeatures.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;


            public CreateProductCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
            {
                var product = new Product();
                product.CategoryId = command.CategoryId;
                product.Barcode = command.Barcode;
                product.Name = command.Name;
                product.Rate = command.Rate;
                product.Description = command.Description;
                product.Price = command.Price;
                product.Quantity = command.Quantity;
                product.CreatedOn = DateTime.Now;
                product.CreatedBy = _currentUserService.Id;
                product.IsDeleted = false;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }
        }
    }
}