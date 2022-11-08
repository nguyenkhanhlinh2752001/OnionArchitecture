using Domain.Contracts;

namespace Domain.Entities
{
    public class ProductDetail: AuditableBaseEntity<int>
    {
        public int ProductId { get; set; }
    }
}