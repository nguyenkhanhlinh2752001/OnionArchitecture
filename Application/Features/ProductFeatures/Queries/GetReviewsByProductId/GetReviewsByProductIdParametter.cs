using Application.Filter;

namespace Application.Features.ProductFeatures.Queries.GetReviewsByProductId
{
    public class GetReviewsByProductIdParametter : PaginationFilter
    {
        public int Rate { get; set; }
        public int ProductId { get; set; }
    }
}