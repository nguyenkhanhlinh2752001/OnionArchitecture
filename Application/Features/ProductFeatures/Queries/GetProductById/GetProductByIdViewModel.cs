using Application.Dtos.Products;

namespace Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal AvgRate { get; set; } = 0;

        public int TotalReview { get; set; } = 0;
        public int SaleAmount { get; set; } = 0;
        public IEnumerable<ProductDetailDto>? ProductDetails { get; set; }

        public IEnumerable<ImageProductDto>? Images { get; set; }
    }
}