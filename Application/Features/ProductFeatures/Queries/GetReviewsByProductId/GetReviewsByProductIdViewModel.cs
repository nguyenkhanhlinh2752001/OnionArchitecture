using Application.Dtos.Reviews;

namespace Application.Features.ProductFeatures.Queries.GetReviewsByProductId
{
    public class GetReviewsByProductIdViewModel
    {
        public int ProductDetailId { get; set; }
        public string UserName { get; set; }
        public string Color { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }
        public int Total { get; set; }
        public IEnumerable<ImageReviewDto> Images { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}