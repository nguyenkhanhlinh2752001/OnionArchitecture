using Domain.Contracts;

namespace Domain.Entities
{
    public class ProductDetail : AuditableBaseEntity<int>
    {
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}