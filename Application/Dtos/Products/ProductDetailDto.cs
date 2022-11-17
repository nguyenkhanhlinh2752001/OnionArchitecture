namespace Application.Dtos.Products
{
    public class ProductDetailDto
    {
        public string Color { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}