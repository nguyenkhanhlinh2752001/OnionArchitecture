namespace Application.Dtos.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductDetailId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }
    }
}