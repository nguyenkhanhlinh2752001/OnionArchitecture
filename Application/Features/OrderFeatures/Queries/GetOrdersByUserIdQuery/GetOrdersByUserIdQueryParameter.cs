using Application.Filter;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery
{
    public class GetOrdersByUserIdQueryParameter : PaginationFilter
    {
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
    }
}