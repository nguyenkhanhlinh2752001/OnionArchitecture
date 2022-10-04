namespace Application.Features.ProductFeatures.Queries.GetProductsSoldQuery
{
    public class GetProductsSoldViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
    }
}