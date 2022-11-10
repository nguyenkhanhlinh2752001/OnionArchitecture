using Domain.Contracts;

namespace Domain.Entities
{
    public class ImageReview : AuditableBaseEntity<int>
    {
        public int ReviewId { get; set; }
        public string Url { get; set; }
    }
}