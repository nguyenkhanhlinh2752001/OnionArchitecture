using System.ComponentModel;

namespace Domain.Contracts
{
    public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
    {
        public TId Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}