namespace Application.Features.ProductFeatures.Queries.GetReviewsByProductId
{
    public class GetReviewsByProductIdViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}