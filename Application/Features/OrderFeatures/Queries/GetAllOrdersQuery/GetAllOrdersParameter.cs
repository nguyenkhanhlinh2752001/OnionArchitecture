using Application.Filter;

namespace Application.Features.OrderFeatures.Queries.GetAllOrdersQuery
{
    public class GetAllOrdersParameter : PaginationFilter
    {
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? TotalPriceFrom { get; set; }
        public decimal? TotalPriceTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}