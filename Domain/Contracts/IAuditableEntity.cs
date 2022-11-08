using System.ComponentModel;

namespace Domain.Contracts
{
    public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
    {
    }

    public interface IAuditableEntity : IEntity
    {
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }

        //public DateTime? DeleledOn { get; set; }
        //public string? DeletedBy { get; set; }
        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}