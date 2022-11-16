using Application.Filter;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByUserIdQuery
{
    public class GetOrdersByUserIdQueryParameter : PaginationFilter
    {
        public decimal? TotalPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}