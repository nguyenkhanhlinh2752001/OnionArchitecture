using Domain.Contracts;

namespace Domain.Entities
{
    public class Cart : AuditableBaseEntity<int>
    {
        public string UserId { get; set; }
    }
}