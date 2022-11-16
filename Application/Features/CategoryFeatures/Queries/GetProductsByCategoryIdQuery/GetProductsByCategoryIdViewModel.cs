namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdViewModel
    {
        public string ProductName { get; set; }
        public decimal? Rate { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }
        public decimal AvgRate { get; set; } = 0;
        public int SaleAmount { get; set; } = 0;
    }
}