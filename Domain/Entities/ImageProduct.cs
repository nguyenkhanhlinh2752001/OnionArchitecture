using Domain.Contracts;

namespace Domain.Entities
{
    public class ImageProduct : AuditableBaseEntity<int>
    {
        public int ProductId { get; set; }
        public string Url { get; set; }
    }
}