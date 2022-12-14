using Domain.Contracts;

namespace Domain.Entities
{
    public class CartDetail : AuditableBaseEntity<int>
    {
        public int CartId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }
}