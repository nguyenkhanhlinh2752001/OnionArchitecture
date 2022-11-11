using Domain.Contracts;

namespace Domain.Entities
{
    public class OrderDetail : AuditableBaseEntity<int>
    {
        public int OrderId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }
}