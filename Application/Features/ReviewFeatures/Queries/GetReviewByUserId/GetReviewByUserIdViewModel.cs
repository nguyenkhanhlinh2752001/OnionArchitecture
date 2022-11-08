namespace Application.Features.ReviewFeatures.Queries.GetReviewByUserId
{
    public class GetReviewByUserIdViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? Rate { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}