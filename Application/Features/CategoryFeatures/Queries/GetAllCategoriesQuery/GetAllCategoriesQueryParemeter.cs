using Application.Filter;

namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryParemeter : PaginationFilter
    {
        public string? Order { get; set; }
        public string? SortBy { get; set; }
        public string? Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}