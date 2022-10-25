namespace Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}