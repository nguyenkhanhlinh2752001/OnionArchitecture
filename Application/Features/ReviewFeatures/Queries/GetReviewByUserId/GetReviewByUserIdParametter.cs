using Application.Filter;

namespace Application.Features.ReviewFeatures.Queries.GetReviewByUserId
{
    public class GetReviewByUserIdParametter : PaginationFilter
    {
        public string? ProductName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Rate { get; set; }
    }
}