using Domain.Contracts;

namespace Domain.Entities
{
    public class Product : AuditableBaseEntity<int>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; } = currentTime();
        public string Description { get; set; }
        public decimal? Rate { get; set; } = 0;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Supplier { get; set; }

        private static string currentTime()
        {
            DateTimeOffset now = DateTime.UtcNow;
            return now.ToString("yyyyMMddHHmmssfff");
        }
    }
}