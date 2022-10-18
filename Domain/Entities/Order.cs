using Domain.Common;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}