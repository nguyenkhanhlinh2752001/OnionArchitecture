using Application.Dtos.Products;

namespace Application.Features.ProductFeatures.Commands.AddEditProduct
{
    public class AddEditProductParameter
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<ProductDetailDto>? ProductDetails { get; set; }
    }
}