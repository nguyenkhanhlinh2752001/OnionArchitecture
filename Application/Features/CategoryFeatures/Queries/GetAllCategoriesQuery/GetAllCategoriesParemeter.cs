using Application.Filter;

namespace Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery
{
    public class GetAllCategoriesParemeter : PaginationFilter
    {
        public string? Name { get; set; }
    }
}