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

        private static string currentTime()
        {
            DateTimeOffset now = DateTime.UtcNow;
            return now.ToString("yyyyMMddHHmmssfff");
        }
    }
}