using Application.Filter;

namespace Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsParameter : PaginationFilter
    {
        public string? ProductName { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public int? FromRate { get; set; }
        public int? ToRate { get; set; }
        public string? CategoryName { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
    }
}