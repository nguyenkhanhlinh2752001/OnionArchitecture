namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdVM
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal? Rate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}