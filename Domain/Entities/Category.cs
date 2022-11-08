using Domain.Common;
using Domain.Contracts;

namespace Domain.Entities
{
    public class Category : AuditableBaseEntity<int>
    {
        public string Name { get; set; }
    }
}