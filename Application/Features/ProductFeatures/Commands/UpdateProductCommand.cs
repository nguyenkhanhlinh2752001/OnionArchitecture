using MediatR;
using Persistence.Context;

namespace Application.Features.ProductFeatures.Commands
{
    public class UpdateProductCommand: IRequest<int>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
        {
            private readonly ApplicationDbContext _context;
            public UpdateProductCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var product = _context.Products.Where(a => a.Id == command.Id).FirstOrDefault();

                if (product == null)
                {
                    return default;
                }
                else
                {
                    product.CategoryId = command.CategoryId;
                    product.Barcode = command.Barcode;
                    product.Name = command.Name;
                    product.Rate = command.Rate;
                    product.Description = command.Description;
                    product.Price = command.Price;
                    product.Quantity = command.Quantity;

                    await _context.SaveChangesAsync();
                    return product.Id;
                }
            }
        }
    }
}
