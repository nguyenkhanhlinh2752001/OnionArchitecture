using Domain.Common;
using Domain.Contracts;

namespace Domain.Entities
{
    public class Order : AuditableBaseEntity<int>
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}