using Application.Filter;

namespace Application.Features.CategoryFeatures.Queries.GetProductsByCategoryIdQuery
{
    public class GetProductsByCategoryIdParamter : PaginationFilter
    {
        public int CategoryId { get; set; }
        public string? Order { get; set; }
        public string? SortBy { get; set; }
    }
}