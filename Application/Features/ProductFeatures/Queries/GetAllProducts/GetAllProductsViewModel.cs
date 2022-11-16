namespace Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? AvgRate { get; set; } = 0;
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int SaleAmount { get; set; } = 0;
        public DateTime? CreatedOn { get; set; }
    }
}