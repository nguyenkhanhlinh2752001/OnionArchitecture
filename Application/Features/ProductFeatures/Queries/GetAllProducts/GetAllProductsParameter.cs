using Application.Filter;

namespace Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsParameter : PaginationFilter
    {
        public string? ProductName { get; set; }
    }
}